using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.Docking
{
    public partial class PrintJobNavigationPanel : DockPanelBase
    {
        internal event Action<PrintJobNavigationNodeTag> SelectedNodeChanged;
        internal event Action<PrintJobNavigationNodeTag> btnRemoveClicked;

        public PrintJobNavigationNodeTag SelectedDataSource
        {
            get
            {
                if (this.trItems.SelectedNode != null && this.trItems.SelectedNode.Tag != null)
                {
                    return this.trItems.SelectedNode.Tag as PrintJobNavigationNodeTag;
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    var selectedNode = ObjectNodeByTag(value);
                    if (selectedNode != null)
                    {
                        SelectNode(selectedNode);

                        if (selectedNode.Parent != null && !selectedNode.Parent.IsExpanded) selectedNode.Parent.Expand();
                        EnableRemoveButton();
                    }
                }
                else if (this.trItems.SelectedNode != null)
                {
                    this.RemoveNodeSelection(this.trItems.SelectedNode);
                    this.trItems.SelectedNode = null;
                }
            }
        }

        public PrintJobNavigationPanel()
        {
            InitializeComponent();

            this.ToolstripIconMouseOver = BrandingManager.DockPanelPrintJobNavigationPropertiesMouseOver;

            foreach (ToolStripItem button in this.toolStrip1.Items)
            {
                button.MouseMove += toolStrip1_MouseMove;
            }

            DisableRemoveButton();
        }

        private void toolStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }

        public TreeNode ModelNodeByIndex(int index)
        {
            foreach (TreeNode modelNode in this.trItems.Nodes)
            {
                var modelNodeTag = modelNode.Tag as PrintJobNavigationNodeTag;
                if (modelNodeTag.ModelIndex == index)
                {
                    return modelNode;
                }
            }

            return null;
        }

        public TreeNode ObjectNodeByTag(PrintJobNavigationNodeTag nodeTag)
        {
            switch (nodeTag.ObjectType)
            {
                case PrintJobNavigationNodeTag.typeOfObject.Model:
                    return ModelNodeByIndex(nodeTag.ModelIndex);
                case PrintJobNavigationNodeTag.typeOfObject.SingleSupportCone:
                    var modelNode = ModelNodeByIndex(nodeTag.ModelIndex);
                    if (modelNode != null)
                    {
                        var singleSupportNode = SingleSupportConesNodeByIndex(modelNode);
                        if (singleSupportNode != null)
                        {
                            var childNodeCount = singleSupportNode.GetNodeCount(false);
                            for (var singleSupportConeNodeIndex = 0; singleSupportConeNodeIndex < childNodeCount; singleSupportConeNodeIndex++)
                            {
                                if (((PrintJobNavigationNodeTag)singleSupportNode.Nodes[singleSupportConeNodeIndex].Tag).ObjectIndex == nodeTag.ObjectIndex)
                                {
                                    return singleSupportNode.Nodes[singleSupportConeNodeIndex];
                                }
                            }
                        }
                    }
                    break;
                case PrintJobNavigationNodeTag.typeOfObject.HorizontalSupportStructure:
                    var modelNodeHorizontalSurface = ModelNodeByIndex(nodeTag.ModelIndex);
                    if (modelNodeHorizontalSurface != null)
                    {
                        var gridSupportNode = GridSupportConesNodeByIndex(modelNodeHorizontalSurface);
                        if (gridSupportNode != null)
                        {
                            var childNodeCount = gridSupportNode.GetNodeCount(false);
                            for (var gridSupportConeNodeIndex = 0; gridSupportConeNodeIndex < childNodeCount; gridSupportConeNodeIndex++)
                            {
                                var gridSupportNodeTag = gridSupportNode.Nodes[gridSupportConeNodeIndex].Tag as PrintJobNavigationNodeTag;
                                if (gridSupportNodeTag.ObjectType == nodeTag.ObjectType && gridSupportNodeTag.ObjectIndex == nodeTag.ObjectIndex)
                                {
                                    return gridSupportNode.Nodes[gridSupportConeNodeIndex];
                                }
                            }
                        }
                    }
                    break;
                case PrintJobNavigationNodeTag.typeOfObject.FlatSupportStructure:
                    var modelNodeFlatSurface = ModelNodeByIndex(nodeTag.ModelIndex);
                    if (modelNodeFlatSurface != null)
                    {
                        var gridSupportNode = GridSupportConesNodeByIndex(modelNodeFlatSurface);
                        if (gridSupportNode != null)
                        {
                            var childNodeCount = gridSupportNode.GetNodeCount(false);
                            for (var gridSupportConeNodeIndex = 0; gridSupportConeNodeIndex < childNodeCount; gridSupportConeNodeIndex++)
                            {
                                var gridSupportNodeTag = gridSupportNode.Nodes[gridSupportConeNodeIndex].Tag as PrintJobNavigationNodeTag;
                                if (gridSupportNodeTag.ObjectType == nodeTag.ObjectType && gridSupportNodeTag.ObjectIndex == nodeTag.ObjectIndex)
                                {
                                    return gridSupportNode.Nodes[gridSupportConeNodeIndex];
                                }
                            }
                        }
                    }
                    break;
            }

            return null;
        }

        public void RemoveNodeSelection(TreeNode node)
        {
            this.trItems.BeforeSelect -= trItems_BeforeSelect;

            var currentSelectedNode = this.trItems.SelectedNode;
            if (currentSelectedNode != null)
            {
                currentSelectedNode.BackColor = this.trItems.BackColor;
                currentSelectedNode.ForeColor = SystemColors.ControlText;
            }

            this.trItems.BeforeSelect += trItems_BeforeSelect;
        }

        public void SelectNode(TreeNode node)
        {
            this.trItems.BeforeSelect -= trItems_BeforeSelect;

            var currentSelectedNode = this.trItems.SelectedNode;
            if (currentSelectedNode != null)
            {
                currentSelectedNode.BackColor = this.trItems.BackColor;
                currentSelectedNode.ForeColor = SystemColors.ControlText;
            }

            if (node != null)
            {
                node.BackColor = SystemColors.Highlight;
                node.ForeColor = SystemColors.HighlightText;

                this.trItems.SelectedNode = node;
            }

            this.trItems.BeforeSelect += trItems_BeforeSelect;
        }

        public TreeNode SingleSupportConesNodeByIndex(TreeNode modelNode)
        {
            foreach (TreeNode childNode in modelNode.Nodes)
            {
                if (childNode.Text.Contains("Single Support"))
                {
                    return childNode;
                }
            }

            return null;
        }

        public TreeNode GridSupportConesNodeByIndex(TreeNode modelNode)
        {
            foreach (TreeNode childNode in modelNode.Nodes)
            {
                if (childNode.Text.Contains("Grid Support"))
                {
                    return childNode;
                }
            }

            return null;
        }

        public void RefreshItems()
        {
            //this.trItems.Nodes.Clear();

            DisableRemoveButton();

            var currentModelIndexes = new List<int>();
            foreach (var object3d in ObjectView.Objects3D)
            {
                if (!(object3d is GroundPane))
                {
                    if (object3d is STLModel3D)
                    {
                        var stlModel = object3d as STLModel3D;
                        if (!currentModelIndexes.Contains(stlModel.Index))
                        {
                            currentModelIndexes.Add(stlModel.Index);
                        }
                    }
                }
            }

            //find obsolete models
            var obsoleteNodes = new List<TreeNode>();
            foreach (TreeNode modelNode in this.trItems.Nodes)
            {
                var modelNodeTag = modelNode.Tag as PrintJobNavigationNodeTag;
                if (!currentModelIndexes.Contains((int)modelNodeTag.ModelIndex))
                {
                    obsoleteNodes.Add(modelNode);
                }
            }

            foreach (var obsoleteNode in obsoleteNodes)
            {
                if (this.trItems.InvokeRequired)
                {
                    this.trItems.Invoke(new MethodInvoker(delegate
                    {
                        this.trItems.Nodes.Remove(obsoleteNode);
                    }));
                }
                else
                {
                    this.trItems.Nodes.Remove(obsoleteNode);
                }
            }


            //update current models
            foreach (var object3d in ObjectView.Objects3D)
            {
                if (!(object3d is GroundPane))
                {
                    var singleSupportConesNode = new TreeNode();
                    var gridSupportConesNode = new TreeNode();
                    if (object3d is STLModel3D)
                    {
                        var stlModel = object3d as STLModel3D;
                        var modelNode = ModelNodeByIndex(stlModel.Index);

                        if (modelNode == null || modelNode.Text == string.Empty)
                        {
                            modelNode = new TreeNode();
                            modelNode.Name = "modelIndex" + stlModel.Index.ToString();
                            modelNode.Text = stlModel.FileName;
                            modelNode.Tag = new PrintJobNavigationNodeTag(PrintJobNavigationNodeTag.typeOfObject.Model, stlModel.Index, 0);

                            singleSupportConesNode.Text = "Single Support";
                            gridSupportConesNode.Text = "Grid Support";
                            modelNode.Nodes.Add(singleSupportConesNode);
                            modelNode.Nodes.Add(gridSupportConesNode);
                            modelNode.Expand();

                            if (this.trItems.InvokeRequired)
                            {
                                this.trItems.Invoke(new MethodInvoker(delegate { this.trItems.Nodes.Add(modelNode); }));
                            }
                            else
                            {
                                this.trItems.Nodes.Add(modelNode);
                            }
                            
                        }
                        else
                        {
                            singleSupportConesNode = SingleSupportConesNodeByIndex(modelNode);
                            gridSupportConesNode = GridSupportConesNodeByIndex(modelNode);
                        }

                        //singleSupportConesNode.Nodes.Clear();
                        //gridSupportConesNode.Nodes.Clear();
                        if (stlModel.SupportStructure != null)
                        {
                            for (var supportConeIndex = 0; supportConeIndex < stlModel.SupportStructure.Count; supportConeIndex++)
                            {
                                var supportConeNode = new TreeNode();
                                supportConeNode.Text = "Support Cone: " + supportConeIndex.ToString();
                                supportConeNode.Tag = new PrintJobNavigationNodeTag(PrintJobNavigationNodeTag.typeOfObject.SingleSupportCone, stlModel.Index, supportConeIndex);
                                //singleSupportConesNode.Nodes.Add(supportConeNode);
                            }
                        }

                        lock (stlModel.Triangles.HorizontalSurfaces.SupportStructure)
                        {
                            var currentSupportConeIndex = 0;
                            var horizontalSurfaceIndex = 0;
                            foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
                            {
                                if (horizontalSurface.SupportStructure.Count > 0)
                                {
                                    var supportConeNode = new TreeNode();
                                    supportConeNode.Text = "Structure: " + currentSupportConeIndex.ToString();
                                    supportConeNode.Tag = new PrintJobNavigationNodeTag(PrintJobNavigationNodeTag.typeOfObject.HorizontalSupportStructure, stlModel.Index, horizontalSurfaceIndex);

                                    if (this.trItems.InvokeRequired)
                                    {
                                        this.trItems.Invoke(new MethodInvoker(delegate
                                        {
                                            gridSupportConesNode.Nodes.Add(supportConeNode);
                                        }));
                                    }
                                    else {
                                        gridSupportConesNode.Nodes.Add(supportConeNode);
                                    }

                                    currentSupportConeIndex++;
                                }

                                horizontalSurfaceIndex++;
                            }

                            var flatSurfaceIndex = 0;
                            foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                            {
                                if (flatSurface.SupportStructure.Count > 0)
                                {
                                    var supportConeNode = new TreeNode();
                                    supportConeNode.Text = "Structure: " + currentSupportConeIndex.ToString();
                                    supportConeNode.Tag = new PrintJobNavigationNodeTag(PrintJobNavigationNodeTag.typeOfObject.FlatSupportStructure, stlModel.Index, flatSurfaceIndex);

                                    if (this.trItems.InvokeRequired)
                                    {
                                        this.trItems.Invoke(new MethodInvoker(delegate
                                        {
                                            gridSupportConesNode.Nodes.Add(supportConeNode);
                                        }));
                                    }
                                    else
                                    {
                                        gridSupportConesNode.Nodes.Add(supportConeNode);
                                    }

                                    currentSupportConeIndex++;
                                }

                                flatSurfaceIndex++;
                            }
                        }

                  //      gridSupportConesNode.Text = string.Format("Grid Support ({0})", gridSupportConesNode.GetNodeCount(false));
                  //      singleSupportConesNode.Text = string.Format("Single Support ({0})", singleSupportConesNode.GetNodeCount(false));
                        
                    }

                }

            }
        }

        private void trItems_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            SelectNode(null);

            if (this.SelectedNodeChanged != null && e.Node != null && e.Node.Tag is PrintJobNavigationNodeTag) {
                var selectedNodeTag = e.Node.Tag as PrintJobNavigationNodeTag;
                SelectedNodeChanged(selectedNodeTag);

                EnableRemoveButton();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DisableRemoveButton();

            if (this.btnRemoveClicked != null && this.trItems.SelectedNode != null && this.trItems.SelectedNode.Tag is PrintJobNavigationNodeTag)
            {
                var selectedNodeTag = this.trItems.SelectedNode.Tag as PrintJobNavigationNodeTag;
                this.btnRemoveClicked(selectedNodeTag);
            }
        }

        private void PrintJobNavigationPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.Enabled)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        this.btnRemove_Click(null, null);
                        break;
                }
            }
        }

        private void DisableRemoveButton()
        {

                this.btnRemove.InvokeEnabled = false;
        }

        private void EnableRemoveButton()
        {
                    this.btnRemove.InvokeEnabled = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.trItems.Nodes.Clear();
            this.RefreshItems();
        }

    }

    public class PrintJobNavigationNodeTag
    {
        internal typeOfObject ObjectType { get; set; }
        internal int ModelIndex { get; set; }
        internal long ObjectIndex { get; set; }

        public enum typeOfObject
        {
            Model = 0,
            SingleSupportCone = 1,
            HorizontalSupportStructure = 2,
            FlatSupportStructure = 3
        }

        public PrintJobNavigationNodeTag()
        {

        }

        public PrintJobNavigationNodeTag(typeOfObject objectType, int modelIndex, long objectIndex)
        {
            this.ObjectType = objectType;
            this.ModelIndex = modelIndex;
            this.ObjectIndex = objectIndex;
        }
    }
}
