﻿using System;
using System.Collections.Generic;

namespace FastReport.Utils
{
#if !COMMUNITY
    /// <summary>
    /// Class for handling Exports visibility in the Preview control.
    /// </summary>
    public partial class ExportsOptions
    {
        private static ExportsOptions instance = null;

        private List<ExportsTreeNode> menuNodes;

        /// <summary>
        /// All exports available in the Preview control.
        /// </summary>
        public List<ExportsTreeNode> ExportsMenu
        {
            get
            {
                RemoveCloudsAndMessengersDuplicatesInMenuNodes();
                return menuNodes;
            }
        }

        private ExportsOptions()
        {
            menuNodes = new List<ExportsTreeNode>();
        }

        private void RemoveCloudsAndMessengersDuplicatesInMenuNodes()
        {
            int last = menuNodes.Count - 1;
            for (int i = last; i >= 0; i--)
            {
                ExportsTreeNode node = menuNodes[i];
                if (node.Name == "Cloud" || node.Name == "Messengers")
                {
                    menuNodes.Remove(node);
                }
            }
        }

        /// <summary>
        /// Gets an instance of ExportOptions.
        /// </summary>
        /// <returns></returns>
        public static ExportsOptions GetInstance()
        {
            if (instance == null)
                instance = new ExportsOptions();

            return instance;
        }

        /// <summary>
        /// Exports menu node.
        /// </summary>
        public partial class ExportsTreeNode
        {
            private const string EXPORT_ITEM_PREFIX = "Export,";
            private const string ITEM_POSTFIX = ",Name";
            private const string EXPORT_ITEM_POSTFIX = ",File";
            private const string CATEGORY_PREFIX = "Export,ExportGroups,";

            private readonly string name;
            private readonly List<ExportsTreeNode> nodes = new List<ExportsTreeNode>();
            private readonly Type exportType = null;
            private readonly int imageIndex = -1;
            private ObjectInfo tag = null;
            private bool enabled = true;
            private readonly bool isExport;

            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name
            {
                get { return name; }
            }

            /// <summary>
            /// Gets nodes.
            /// </summary>
            public List<ExportsTreeNode> Nodes
            {
                get { return nodes; }
            }
            
            /// <summary>
            /// Gets type of the export.
            /// </summary>
            public Type ExportType
            {
                get { return exportType; }
            }
            
            /// <summary>
            /// Gets index of the image.
            /// </summary>
            public int ImageIndex
            {
                get { return imageIndex; }
            }
            
            /// <summary>
            /// Gets or sets the tag.
            /// </summary>
            public ObjectInfo Tag
            {
                get { return tag; }
                set { tag = value; }
            }

            /// <summary>
            /// Gets or sets a value that indicates is node enabled.
            /// </summary>
            public bool Enabled
            {
                get { return enabled; }
                set { enabled = value; }
            }
            
            /// <summary>
            /// Gets true if node is export, otherwise false.
            /// </summary>
            public bool IsExport
            {
                get { return isExport; }
            }

            public ExportsTreeNode(string name, bool isExport)
            {
                this.name = name;
                this.isExport = isExport;
            }

            public ExportsTreeNode(string name, Type exportType, bool isExport)
                : this(name, isExport)
            {
                this.exportType = exportType;
            }

            public ExportsTreeNode(string name, Type exportType, bool isExport, ObjectInfo tag) 
                : this(name, exportType, isExport)
            {
                this.tag = tag;
            }

            public ExportsTreeNode(string name, int imageIndex, bool isExport)
                : this(name, isExport)
            {
                this.imageIndex = imageIndex;
            }

            public ExportsTreeNode(string name, Type exportType, int imageIndex, bool isExport)
                : this(name, exportType, isExport)
            {
                this.imageIndex = imageIndex;
            }

            public ExportsTreeNode(string name, Type exportType, int imageIndex, bool isExport, ObjectInfo tag)
                : this(name, exportType, imageIndex, isExport)
            {
                this.tag = tag;
            }

            public ExportsTreeNode(string name, Type exportType, int imageIndex, bool enabled, bool isExport)
                : this(name, exportType, imageIndex, isExport)
            {
                this.enabled = enabled;
            }
        }

        /// <summary>
        /// Saves current visible exports in config file.
        /// </summary>
        public void SaveExportOptions()
        {
            SaveOptions();
        }

        /// <summary>
        /// Restores visible exports from config file.
        /// </summary>
        public void RestoreExportOptions()
        {
            RestoreOptions();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RegisterExports()
        {
            Queue<ExportsTreeNode> queue = new Queue<ExportsTreeNode>(menuNodes);

            while (queue.Count != 0)
            {
                ExportsTreeNode node = queue.Dequeue();
                ObjectInfo tag = null;
                if (node.ExportType != null)
                {
                    tag = RegisteredObjects.AddExport(node.ExportType, node.ToString(), node.ImageIndex);
                }
                node.Tag = tag;
                foreach (ExportsTreeNode nextNode in node.Nodes)
                {
                    queue.Enqueue(nextNode);
                }
            }
        }

        private ExportsTreeNode FindItem(string name, Type exportType)
        {
            Queue<ExportsTreeNode> queue = new Queue<ExportsTreeNode>(menuNodes);

            while (queue.Count != 0)
            {
                ExportsTreeNode node = queue.Dequeue();

                if (exportType != null && node.ExportType == exportType ||
                    !string.IsNullOrEmpty(name) && node.Name == name)
                {
                    return node;
                }

                foreach (ExportsTreeNode nextNode in node.Nodes)
                {
                    queue.Enqueue(nextNode);
                }
            }

            return null;
        }

        /// <summary>
        /// Sets Export category visibility.
        /// </summary>
        /// <param name="name">Export category name.</param>
        /// <param name="enabled">Visibility state.</param>
        public void SetExportCategoryEnabled(string name, bool enabled)
        {
            FindItem(name, null).Enabled = enabled;
        }

        /// <summary>
        /// Sets Export visibility.
        /// </summary>
        /// <param name="exportType">Export type.</param>
        /// <param name="enabled">Visibility state.</param>
        public void SetExportEnabled(Type exportType, bool enabled)
        {
            ExportsTreeNode node = FindItem(null, exportType);
            if (node != null)
            {
                node.Enabled = enabled;
            }
        }
    }
#endif
}
