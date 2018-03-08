/**********************************************************
** 文件名： GraphicsList.cs
** 文件作用:
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;

#endregion Using directives

namespace DrawTools
{
    using DrawTools.Command;
    using DrawTools.Device;
    using DrawTools.DocToolkit;
    using DrawTools.Model;
    using DrawList = List<DrawTools.DocToolkit.DrawObject>;

    /// <summary>
    /// List of graphic objects
    /// </summary>
    [Serializable]
    public class GraphicsList : ISerializable
    {
        #region Members

        private DrawList graphicsList;

        private List<string> _ipverify;

        public List<string> IPVerify
        {
            get { return _ipverify; }
            set { _ipverify = value; }
        }

        private List<DrawObject> _copyList;

        public List<DrawObject> CopyList
        {
            get { return _copyList; }
            set { _copyList = value; }
        }

        private List<string> _bomverify;

        public List<string> BomVerify
        {
            get { return _bomverify; }
            set { _bomverify = value; }
        }

        private List<string> _agmverify;

        public List<string> AGMVerify
        {
            get { return _agmverify; }
            set { _agmverify = value; }
        }

        //private List<string> _agmdualverify;
        //public List<string> AGMDualVerify
        //{
        //    get { return _agmdualverify; }
        //    set { _agmdualverify = value; }
        //}

        private List<string> _agmdWallSingleverify;

        public List<string> AGMWallSingleVerify
        {
            get { return _agmdWallSingleverify; }
            set { _agmdWallSingleverify = value; }
        }

        private List<string> _aGMWallDualverify;

        public List<string> AGMWallDualVerify
        {
            get { return _aGMWallDualverify; }
            set { _aGMWallDualverify = value; }
        }

        private List<string> _aGMWallDummyverify;

        public List<string> AGMWallDummyVerify
        {
            get { return _aGMWallDummyverify; }
            set { _aGMWallDummyverify = value; }
        }

        private List<string> _tcmverify;

        public List<string> TCMVerify
        {
            get { return _tcmverify; }
            set { _tcmverify = value; }
        }

        private List<string> _tvmverify;

        public List<string> TVMVerify
        {
            get { return _tvmverify; }
            set { _tvmverify = value; }
        }

        private List<string> _scverify;

        public List<string> SCVerify
        {
            get { return _scverify; }
            set { _scverify = value; }
        }

        private List<string> _switchverify;

        public List<string> SwitchVerify
        {
            get { return _switchverify; }
            set { _switchverify = value; }
        }

        private List<string> _paidverify;

        public List<string> PaidVerify
        {
            get { return _paidverify; }
            set { _paidverify = value; }
        }

        private List<string> _arrayverify;

        public List<string> ArrayVerify
        {
            get { return _arrayverify; }
            set { _arrayverify = value; }
        }

        private const string entryCount = "Count";
        private const string entryType = "Type";

        private System.Collections.ArrayList SelectionArray = new System.Collections.ArrayList();

        #endregion Members

        #region Constructor

        public GraphicsList()
        {
            graphicsList = new DrawList();
            _ipverify = new List<string>();
            _bomverify = new List<string>();
            _agmverify = new List<string>();
            //_agmdualverify = new List<string>();
            _tcmverify = new List<string>();
            _tvmverify = new List<string>();
            _copyList = new List<DrawObject>();
            _scverify = new List<string>();
            _agmdWallSingleverify = new List<string>();
            _aGMWallDualverify = new List<string>();
            _aGMWallDummyverify = new List<string>();
            _switchverify = new List<string>();
            _paidverify = new List<string>();
            _arrayverify = new List<string>();
        }

        #endregion Constructor

        #region Serialization Support

        protected GraphicsList(SerializationInfo info, StreamingContext context)
        {
            graphicsList = new DrawList();

            int n = info.GetInt32(entryCount);
            string typeName;
            DrawObject drawObject;

            for (int i = 0; i < n; i++)
            {
                typeName = info.GetString(
                    String.Format(CultureInfo.InvariantCulture,
                        "{0}{1}",
                    entryType, i));

                drawObject = (DrawObject)Assembly.GetExecutingAssembly().CreateInstance(
                    typeName);

                drawObject.LoadFromStream(info, i);

                graphicsList.Add(drawObject);
            }
        }

        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(entryCount, graphicsList.Count);

            int i = 0;

            foreach (DrawObject o in graphicsList)
            {
                info.AddValue(
                    String.Format(CultureInfo.InvariantCulture,
                        "{0}{1}",
                        entryType, i),
                    o.GetType().FullName);

                o.SaveToStream(info, i);

                i++;
            }
        }

        #endregion Serialization Support

        #region Other functions

        public void Draw(Graphics g)
        {
            int n = graphicsList.Count;
            DrawObject o;

            // Enumerate list in reverse order to get first
            // object on the top of Z-order.
            for (int i = n - 1; i >= 0; i--)
            {
                o = graphicsList[i];

                o.Draw(g);

                if (o.Selected == true)
                {
                    o.DrawTracker(g);
                }
            }
        }

        /// <summary>
        /// 初始化设备缓存字典
        /// </summary>
        /// <param name="node"></param>
        public void SetDictionary(StationMapModel node)
        {
            string deviceTotalIdStr = node.DeviceID;//.Substring(node.DeviceID.Length-2);

            if (node.DeviceType == "02") //bom
            {
                _bomverify.Add(deviceTotalIdStr);
                _ipverify.Add(node.IpAdd);
            }
            else if (node.DeviceType == "11") //server
            {
                _scverify.Add(deviceTotalIdStr);
                _ipverify.Add(node.IpAdd);
            }
            else if (node.DeviceType == "01") //TVM
            {
                _tvmverify.Add(deviceTotalIdStr);
                _ipverify.Add(node.IpAdd);
            }
            else if (node.DeviceType == "04")  //TCM
            {
                _tcmverify.Add(deviceTotalIdStr);
                _ipverify.Add(node.IpAdd);
            }
            else if (node.DeviceType == "12")  //switch
            {
                _switchverify.Add(deviceTotalIdStr);
            }
            else if (node.DeviceType == "13")  //sc_network_device_port 13
            {
                //TODO:未添加网络端口
            }
            else if (node.DeviceType == "06") //06AGM
            {
                _agmverify.Add(deviceTotalIdStr);
                _ipverify.Add(node.IpAdd);
            }
            else if (node.DeviceType == "85")
            {
                _agmdWallSingleverify.Add(deviceTotalIdStr);
            }
            else if (node.DeviceType == "86")
            {
                _aGMWallDualverify.Add(deviceTotalIdStr);
            }
            else if (node.DeviceType == "87")
            {
                _aGMWallDummyverify.Add(deviceTotalIdStr);
            }
            else if (node.DeviceType == "88")
            {
                _paidverify.Add(deviceTotalIdStr);
            }
            else if (node.DeviceType == "89")
            {
                _arrayverify.Add(deviceTotalIdStr);
            }
        }

        public void showproperty(bool check)
        {
            int n = graphicsList.Count;

            for (int i = 0; i < n; i++)
            {
                ((DrawObject)graphicsList[i]).ShowItemProperty(check);
            }
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public void Dump()
        {
            Trace.WriteLine("");

            foreach (DrawObject o in graphicsList)
            {
                o.Dump();
            }
        }

        /// <summary>
        /// Clear all objects in the list
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool Clear()
        {
            bool result = (graphicsList.Count > 0);
            graphicsList.Clear();
            return result;
        }

        /// <summary>
        /// Count and this [nIndex] allow to read all graphics objects
        /// from GraphicsList in the loop.
        /// </summary>
        public int Count
        {
            get
            {
                return graphicsList.Count;
            }
        }

        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= graphicsList.Count)
                    return null;

                return graphicsList[index];
            }
        }

        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;

                foreach (DrawObject o in Selection)
                {
                    n++;
                }

                return n;
            }
        }

        /// <summary>
        /// Returns INumerable object which may be used for enumeration
        /// of selected objects.
        ///
        /// Note: returning IEnumerable<DrawObject> breaks CLS-compliance
        /// (assembly CLSCompliant = true is removed from AssemblyInfo.cs).
        /// To make this program CLS-compliant, replace
        /// IEnumerable<DrawObject> with IEnumerable. This requires
        /// casting to object at runtime.
        /// </summary>
        /// <value></value>
        public IEnumerable<DrawObject> Selection
        {
            get
            {
                foreach (DrawObject o in graphicsList)
                {
                    if (o.Selected)
                    {
                        yield return o;
                    }
                }
            }
        }

        public System.Collections.ArrayList ASelection
        {
            get
            {
                SelectionArray.Clear();
                foreach (DrawObject o in graphicsList)
                {
                    if (o.Selected)
                    {
                        SelectionArray.Add(o);
                    }
                }
                return SelectionArray;
            }
        }

        public DrawObject SelectObject
        {
            get
            {
                foreach (DrawObject o in graphicsList)
                {
                    if (o.Selected)
                    {
                        return o;
                    }
                }
                return null;
            }
        }

        public void Add(DrawObject obj)
        {
            // insert to the top of z-order
            graphicsList.Insert(0, obj);
        }

        /// <summary>
        /// Insert object to specified place.
        /// Used for Undo.
        /// </summary>
        public void Insert(int index, DrawObject obj)
        {
            if (index >= 0 && index < graphicsList.Count)
            {
                graphicsList.Insert(index, obj);
            }
        }

        /// <summary>
        /// Replace object in specified place.
        /// Used for Undo.
        /// </summary>
        public void Replace(int index, DrawObject obj)
        {
            if (index >= 0 && index < graphicsList.Count)
            {
                graphicsList.RemoveAt(index);
                graphicsList.Insert(index, obj);
            }
        }

        /// <summary>
        /// Remove object by index.
        /// Used for Undo.
        /// </summary>
        public void RemoveAt(int index)
        {
            graphicsList.RemoveAt(index);
        }

        /// <summary>
        /// Delete last added object from the list
        /// (used for Undo operation).
        /// </summary>
        public void DeleteLastAddedObject()
        {
            if (graphicsList.Count > 0)
            {
                graphicsList.RemoveAt(0);
            }
        }

        public void SelectInRectangle(Rectangle rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in graphicsList)
            {
                if (o.IntersectsWith(rectangle))
                    o.Selected = true;
            }
        }

        public void UnselectAll()
        {
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = false;
            }
        }

        public void SelectAll()
        {
            int n = graphicsList.Count;
            foreach (DrawObject o in graphicsList)
            {
                o.Selected = true;
            }
        }

        public void SelectDevice(int tagid)
        {
            foreach (DrawObject o in graphicsList)
            {
                switch (o.GetType().Name)
                {
                    case "BOM":
                        BOM bomDevice = (BOM)o;
                        if (bomDevice.TagIDBase.Equals(tagid))
                        {
                            bomDevice.Selected = true;
                        }
                        break;

                    case "AGMChannel":
                        AGMChannel agmDevice = (AGMChannel)o;
                        if (agmDevice.TagIDBase.Equals(tagid))
                        {
                            agmDevice.Selected = true;
                        }
                        break;

                    case "AGMChannelDual":
                        AGMChannelDual agmDualDevice = (AGMChannelDual)o;
                        if (agmDualDevice.TagIDBase.Equals(tagid))
                        {
                            agmDualDevice.Selected = true;
                        }
                        break;

                    case "TCM":
                        TCM tcm = (TCM)o;
                        if (tcm.TagIDBase.Equals(tagid))
                        {
                            tcm.Selected = true;
                        }
                        break;

                    case "TVM":
                        TVM tvm = (TVM)o;
                        if (tvm.TagIDBase.Equals(tagid))
                        {
                            tvm.Selected = true;
                        }
                        break;

                    case "SC":
                        SC sc = (SC)o;
                        if (sc.TagIDBase.Equals(tagid))
                        {
                            sc.Selected = true;
                        }
                        break;
                }
            }
        }

        public void SelectAlist(int oTagID)
        {
            foreach (DrawObject o in graphicsList)
            {
                if (o.GetType().Name.Equals("Text"))
                {
                    Text t = (Text)o;
                    if (t.ObjectID.Equals(oTagID))
                    {
                        t.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool DeleteSelection()
        {
            bool result = false;

            int n = graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)graphicsList[i]).Selected)
                {
                    graphicsList.RemoveAt(i);
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Move selected items to front (beginning of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToFront()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((graphicsList[i]).Selected)
                {
                    tempList.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for (i = 0; i < n; i++)
            {
                graphicsList.Insert(0, tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Move selected items to back (end of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToBack()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((graphicsList[i]).Selected)
                {
                    tempList.Add(graphicsList[i]);
                    graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for (i = n - 1; i >= 0; i--)
            {
                graphicsList.Add(tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Get properties from selected objects and fill GraphicsProperties instance
        /// </summary>
        /// <returns></returns>
        private GraphicsProperties GetProperties()
        {
            GraphicsProperties properties = new GraphicsProperties();

            bool bFirst = true;

            int firstColor = 0;
            int firstPenWidth = 1;

            bool allColorsAreEqual = true;
            bool allWidthAreEqual = true;

            foreach (DrawObject o in Selection)
            {
                if (bFirst)
                {
                    firstColor = o.Color.ToArgb();
                    firstPenWidth = o.PenWidth;
                    bFirst = false;
                }
                else
                {
                    if (o.Color.ToArgb() != firstColor)
                        allColorsAreEqual = false;

                    if (o.PenWidth != firstPenWidth)
                        allWidthAreEqual = false;
                }
            }

            if (allColorsAreEqual)
            {
                properties.Color = Color.FromArgb(firstColor);
            }

            if (allWidthAreEqual)
            {
                properties.PenWidth = firstPenWidth;
            }

            return properties;
        }

        /// <summary>
        /// Apply properties for all selected objects.
        /// Returns TRue if at least one property is changed.
        /// </summary>
        private bool ApplyProperties(GraphicsProperties properties)
        {
            bool changed = false;

            foreach (DrawObject o in graphicsList)
            {
                if (o.Selected)
                {
                    if (properties.Color.HasValue)
                    {
                        if (o.Color != properties.Color.Value)
                        {
                            o.Color = properties.Color.Value;
                            DrawObject.LastUsedColor = properties.Color.Value;
                            changed = true;
                        }
                    }

                    if (properties.PenWidth.HasValue)
                    {
                        if (o.PenWidth != properties.PenWidth.Value)
                        {
                            o.PenWidth = properties.PenWidth.Value;
                            DrawObject.LastUsedPenWidth = properties.PenWidth.Value;
                            changed = true;
                        }
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Show Properties dialog. Return true if list is changed
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool ShowPropertiesDialog(DrawArea parent)
        {
            if (SelectionCount < 1)
                return false;

            GraphicsProperties properties = GetProperties();
            PropertiesDialog dlg = new PropertiesDialog();
            dlg.Properties = properties;

            CommandChangeState c = new CommandChangeState(this);

            if (dlg.ShowDialog(parent) != DialogResult.OK)
                return false;

            if (ApplyProperties(properties))
            {
                c.NewState(this);
                parent.AddCommandToHistory(c);
            }

            return true;
        }

        #endregion Other functions
    }
}