using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace Atum.Studio.Controls
{

    /// <summary>
    /// This class overrides the standard PropertyGrid provided by Microsoft.
    /// It also allows to hide (or filter) the properties of the SelectedObject displayed by the PropertyGrid.
    /// </summary>
    public partial class PropertyGridFiltered : PropertyGrid
    {
        List<PropertyDescriptor> _propertyDescriptors = new List<PropertyDescriptor>();
        private AttributeCollection _hiddenAttributes = null, _browsableAttributes = null;
        private string[] _browsableProperties = null, _hiddenProperties = null;
        private Dictionary<string, string> _renamedProperties = null;
        private PropertyGridObjectWrapper _wrapper = null;

        public void SetParent(Form form)
        {
            // Catch null arguments
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }

            // Set this property to intercept all events
            form.KeyPreview = true;

            // Listen for keydown event
            form.KeyDown += new KeyEventHandler(this.Form_KeyDown);
        }

        /// <summary>Public constructor.</summary>
        /// 

        public PropertyGridFiltered()
        {
            InitializeComponent();
            base.SelectedObject = _wrapper;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // Exit if cursor not in control
           if (!this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
            {
                return;
            }

            // Handle tab key
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                // Get selected griditem
                GridItem gridItem = this.SelectedGridItem;
                if (gridItem == null) { return; }

                // Create a collection all visible child griditems in propertygrid
                GridItem root = gridItem;
                while (root.GridItemType != GridItemType.Root)
                {
                    root = root.Parent;
                }
                List<GridItem> gridItems = new List<GridItem>();
                this.FindItems(root, gridItems);

                // Get position of selected griditem in collection
                int index = gridItems.IndexOf(gridItem);

                // Select next griditem in collection
                if (index < gridItems.Count - 1)
                {
                    this.SelectedGridItem = gridItems[++index];
                }
            }
        }
        private void FindItems(GridItem item, List<GridItem> gridItems)
        {
            switch (item.GridItemType)
            {
                case GridItemType.Root:
                case GridItemType.Category:
                    foreach (GridItem i in item.GridItems)
                    {
                        this.FindItems(i, gridItems);
                    }
                    break;
                case GridItemType.Property:
                    gridItems.Add(item);
                    if (item.Expanded)
                    {
                        foreach (GridItem i in item.GridItems)
                        {
                            this.FindItems(i, gridItems);
                        }
                    }
                    break;
                case GridItemType.ArrayValue:
                    break;
            }
        }


        public new AttributeCollection BrowsableAttributes
        {
            get { return _browsableAttributes; }
            set
            {
                if (_browsableAttributes != value)
                {
                    _hiddenAttributes = null;
                    _browsableAttributes = value;
                }
            }
        }

        /// <summary>Get or set the categories to hide.</summary>
        public AttributeCollection HiddenAttributes
        {
            get { return _hiddenAttributes; }
            set
            {
                if (value != _hiddenAttributes)
                {
                    _hiddenAttributes = value;
                    _browsableAttributes = null;
                }
            }
        }
        /// <summary>Get or set the properties to show.</summary>
        /// <exception cref="ArgumentException">if one or several properties don't exist.</exception>
        public string[] BrowsableProperties
        {
            get { return _browsableProperties; }
            set
            {
                if (value != _browsableProperties)
                {
                    _browsableProperties = value;
                }
            }
        }

        /// <summary>Get or set the properties to show.</summary>
        /// <exception cref="ArgumentException">if one or several properties don't exist.</exception>
        public Dictionary<string, string> RenamedProperties
        {
            get { return this._renamedProperties; }
            set
            {
                if (value != _renamedProperties)
                {
                    _renamedProperties = value;
                }
            }
        }

        /// <summary>Get or set the properties to hide.</summary>
        public string[] HiddenProperties
        {
            get { return _hiddenProperties; }
            set
            {
                if (value != _hiddenProperties)
                {
                    _hiddenProperties = value;
                }
            }
        }

        /// <summary>Overwrite the PropertyGrid.SelectedObject property.</summary>
        /// <remarks>The object passed to the base PropertyGrid is the wrapper.</remarks>
        public new object SelectedObject
        {
            get { return _wrapper != null ? ((PropertyGridObjectWrapper)base.SelectedObject).SelectedObject : null; }
            set
            {
                // Set the new object to the wrapper and create one if necessary.
                if (_wrapper == null)
                {
                    _wrapper = new PropertyGridObjectWrapper(value);
                    RefreshProperties();
                }
                else if (_wrapper.SelectedObject != value)
                {
                    bool needrefresh = value.GetType() != _wrapper.SelectedObject.GetType();
                    _wrapper.SelectedObject = value;
                    if (needrefresh) RefreshProperties();
                }
                // Set the list of properties to the wrapper.
                _wrapper.PropertyDescriptors = _propertyDescriptors;
                // Link the wrapper to the parent PropertyGrid.
                if (this.InvokeRequired)
                {
                    base.Invoke(new MethodInvoker(delegate { base.SelectedObject = _wrapper; }));
                }
                else
                {
                    base.SelectedObject = _wrapper;
                }

            }
        }

        /// <summary>Called when the browsable properties have changed.</summary>
        private void OnBrowsablePropertiesChanged()
        {
            if (_wrapper == null) return;
        }

        /// <summary>Build the list of the properties to be displayed in the PropertyGrid, following the filters defined the Browsable and Hidden properties.</summary>
        public void RefreshProperties()
        {
            if (_wrapper == null) return;
            // Clear the list of properties to be displayed.
            _propertyDescriptors.Clear();
            // Check whether the list is filtered 
            if (_browsableAttributes != null && _browsableAttributes.Count > 0)
            {
                // Add to the list the attributes that need to be displayed.
                foreach (Attribute attribute in _browsableAttributes) ShowAttribute(attribute);
            }
            else
            {
                // Fill the collection with all the properties.
                PropertyDescriptorCollection originalpropertydescriptors = TypeDescriptor.GetProperties(_wrapper.SelectedObject);
                foreach (PropertyDescriptor propertydescriptor in originalpropertydescriptors) _propertyDescriptors.Add(propertydescriptor);
                // Remove from the list the attributes that mustn't be displayed.
                if (_hiddenAttributes != null) foreach (Attribute attribute in _hiddenAttributes) HideAttribute(attribute);
            }
            // Get all the properties of the SelectedObject
            PropertyDescriptorCollection allproperties = TypeDescriptor.GetProperties(_wrapper.SelectedObject);
            // Hide if necessary, some properties
            if (_hiddenProperties != null && _hiddenProperties.Length > 0)
            {
                // Remove from the list the properties that mustn't be displayed.
                foreach (string propertyname in _hiddenProperties)
                {
                    try
                    {
                        PropertyDescriptor property = allproperties[propertyname];
                        // Remove from the list the property
                        HideProperty(property);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                }
            }
            // Display if necessary, some properties
            if (_browsableProperties != null && _browsableProperties.Length > 0)
            {
                foreach (string propertyname in _browsableProperties)
                {
                    try
                    {
                        ShowProperty(allproperties[propertyname]);
                    }
                    catch (Exception knfe)
                    {
                        Debug.WriteLine(knfe.Message);
                        throw new ArgumentException("Property not found", propertyname);
                    }
                }
            }

            //Rename display names
            // Get all the properties of the SelectedObject
            // Hide if necessary, some properties
            if (this._renamedProperties != null && this._renamedProperties.Count > 0)
            {
                foreach (var propertyKeyValue in _renamedProperties)
                {
                    try
                    {
                        PropertyDescriptor property = allproperties[propertyKeyValue.Key];
                        //Rename display name
                        RenameProperty(property, propertyKeyValue.Value);

                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                }
                //this._renamedProperties.Clear();
            }

        }
        private void RenameProperty(PropertyDescriptor property, string newName)
        {
            DisplayNameAttribute attribute = (DisplayNameAttribute)
                                          property.Attributes[typeof(DisplayNameAttribute)];
            System.Reflection.FieldInfo fieldToChange = attribute.GetType().GetField("_displayName",
                System.Reflection.BindingFlags.IgnoreCase |
                                             System.Reflection.BindingFlags.NonPublic |
                                             System.Reflection.BindingFlags.Instance);

            if (fieldToChange != null) fieldToChange.SetValue(attribute, newName);
        }
        /// <summary>Allows to hide a set of properties to the parent PropertyGrid.</summary>
        /// <param name="propertyname">A set of attributes that filter the original collection of properties.</param>
        /// <remarks>For better performance, include the BrowsableAttribute with true value.</remarks>
        private void HideAttribute(Attribute attribute)
        {
            PropertyDescriptorCollection filteredoriginalpropertydescriptors = TypeDescriptor.GetProperties(_wrapper.SelectedObject, new Attribute[] { attribute });
            if (filteredoriginalpropertydescriptors == null || filteredoriginalpropertydescriptors.Count == 0) throw new ArgumentException("Attribute not found", attribute.ToString());
            foreach (PropertyDescriptor propertydescriptor in filteredoriginalpropertydescriptors) HideProperty(propertydescriptor);
        }
        /// <summary>Add all the properties that match an attribute to the list of properties to be displayed in the PropertyGrid.</summary>
        /// <param name="property">The attribute to be added.</param>
        private void ShowAttribute(Attribute attribute)
        {
            PropertyDescriptorCollection filteredoriginalpropertydescriptors = TypeDescriptor.GetProperties(_wrapper.SelectedObject, new Attribute[] { attribute });
            if (filteredoriginalpropertydescriptors == null || filteredoriginalpropertydescriptors.Count == 0) throw new ArgumentException("Attribute not found", attribute.ToString());
            foreach (PropertyDescriptor propertydescriptor in filteredoriginalpropertydescriptors) ShowProperty(propertydescriptor);
        }
        /// <summary>Add a property to the list of properties to be displayed in the PropertyGrid.</summary>
        /// <param name="property">The property to be added.</param>
        private void ShowProperty(PropertyDescriptor property)
        {
            if (!_propertyDescriptors.Contains(property)) _propertyDescriptors.Add(property);
        }
        /// <summary>Allows to hide a property to the parent PropertyGrid.</summary>
        /// <param name="propertyname">The name of the property to be hidden.</param>
        private void HideProperty(PropertyDescriptor property)
        {
            if (_propertyDescriptors.Contains(property)) _propertyDescriptors.Remove(property);
        }
    }
}
