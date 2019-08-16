using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Atum.Studio.Controls
{
        public class PropertyGridObjectWrapper : ICustomTypeDescriptor
        {
            /// <summary>Contain a reference to the selected objet that will linked to the parent PropertyGrid.</summary>
            private object _selectedObject = null;
            /// <summary>Contain a reference to the collection of properties to show in the parent PropertyGrid.</summary>
            /// <remarks>By default, m_PropertyDescriptors contain all the properties of the object. </remarks>
            List<PropertyDescriptor> _propertyDescriptors = new List<PropertyDescriptor>();

            /// <summary>Simple constructor.</summary>
            /// <param name="obj">A reference to the selected object that will linked to the parent PropertyGrid.</param>
            public PropertyGridObjectWrapper(object obj)
            {
                _selectedObject = obj;
            }

            /// <summary>Get or set a reference to the selected objet that will linked to the parent PropertyGrid.</summary>
            public object SelectedObject
            {
                get { return _selectedObject; }
                set { if (_selectedObject != value) _selectedObject = value; }
            }

            /// <summary>Get or set a reference to the collection of properties to show in the parent PropertyGrid.</summary>
            public List<PropertyDescriptor> PropertyDescriptors
            {
                get { return _propertyDescriptors; }
                set { _propertyDescriptors = value; }
            }

            #region ICustomTypeDescriptor Members
            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                return GetProperties();
            }

            public PropertyDescriptorCollection GetProperties()
            {
                return new PropertyDescriptorCollection(_propertyDescriptors.ToArray(), true);
            }

            /// <summary>GetAttributes.</summary>
            /// <returns>AttributeCollection</returns>
            public AttributeCollection GetAttributes()
            {
                return TypeDescriptor.GetAttributes(_selectedObject, true);
            }
            /// <summary>Get Class Name.</summary>
            /// <returns>String</returns>
            public String GetClassName()
            {
                return TypeDescriptor.GetClassName(_selectedObject, true);
            }
            /// <summary>GetComponentName.</summary>
            /// <returns>String</returns>
            public String GetComponentName()
            {
                return TypeDescriptor.GetComponentName(_selectedObject, true);
            }

            /// <summary>GetConverter.</summary>
            /// <returns>TypeConverter</returns>
            public TypeConverter GetConverter()
            {
                return TypeDescriptor.GetConverter(_selectedObject, true);
            }

            /// <summary>GetDefaultEvent.</summary>
            /// <returns>EventDescriptor</returns>
            public EventDescriptor GetDefaultEvent()
            {
                return TypeDescriptor.GetDefaultEvent(_selectedObject, true);
            }

            /// <summary>GetDefaultProperty.</summary>
            /// <returns>PropertyDescriptor</returns>
            public PropertyDescriptor GetDefaultProperty()
            {
                return TypeDescriptor.GetDefaultProperty(_selectedObject, true);
            }

            /// <summary>GetEditor.</summary>
            /// <param name="editorBaseType">editorBaseType</param>
            /// <returns>object</returns>
            public object GetEditor(Type editorBaseType)
            {
                return TypeDescriptor.GetEditor(this, editorBaseType, true);
            }

            public EventDescriptorCollection GetEvents(Attribute[] attributes)
            {
                return TypeDescriptor.GetEvents(_selectedObject, attributes, true);
            }

            public EventDescriptorCollection GetEvents()
            {
                return TypeDescriptor.GetEvents(_selectedObject, true);
            }

            public object GetPropertyOwner(PropertyDescriptor pd)
            {
                return _selectedObject;
            }

            #endregion

        }
    }