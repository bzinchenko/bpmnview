using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BPMN.View
{
  public partial class ElementsForm : Form
  {
    private Model model;

    public ElementsForm(Model mdl)
    {
      InitializeComponent();
      model = mdl;
    }

    private void ElementsForm_Load(object sender, EventArgs e)
    {
      if (model != null)
        gridElements.DataSource = ElementsTable(model.Elements);
    }

    private void gridElements_SelectionChanged(object sender, EventArgs e)
    {
      ViewElement(true);
    }

    private void gridSubElements_SelectionChanged(object sender, EventArgs e)
    {
      //ViewElement(false);
    }

    private void ViewElement(bool primary) 
    {
      gridSubElements.DataSource = null;
      gridAttributes.DataSource = null;
      if(primary) gridProperties.DataSource = null;

      if (gridElements.SelectedRows.Count > 0)
      {
        string id = gridElements.SelectedRows[0].Cells["ID"].Value.ToString();
        Element el = ElementByID(id);
        if (el != null)
        {
          if (primary)
          {
            if (el.Elements != null)
            {
              List<Element> elm = new List<Element>();
              foreach (var elt in el.Elements)
              {
                if (elt.Value != null)
                  elm.AddRange(elt.Value);
              }
              gridSubElements.DataSource = ElementsTable(elm);
            }
          }

          if (el.Properties != null)
          {
            Dictionary<string, string> props = new Dictionary<string, string>();
            foreach (var prop in el.Properties)
            {
              string propList = "";
              foreach (string pr in prop.Value)
              {
                if (!string.IsNullOrEmpty(propList))
                  propList += ", ";
                propList += pr;
              }
              props.Add(prop.Key, propList);
            }
          }

          gridAttributes.DataSource = StringTable(el.Attributes);
        }
      }
    }

    private DataTable ElementsTable(IEnumerable<Element> elements)
    {
      DataTable table = new DataTable();
      table.Columns.Add("Name", typeof(string));
      table.Columns.Add("TypeName", typeof(string));
      table.Columns.Add("ID", typeof(string));
      table.Columns.Add("ParentID", typeof(string));

      if (elements == null) 
        return null;

      table.BeginLoadData();
      foreach (Element el in elements)
      {
        if (el.TypeName.Contains("BPMN")) continue;
        DataRow row = table.NewRow();
        if (el.Attributes.ContainsKey("name"))
          row["Name"] = el.Attributes["name"];
        row["TypeName"] = el.TypeName;
        if (el.Attributes.ContainsKey("id"))
          row["ID"] = el.Attributes["id"];
        row["ParentID"] = el.ParentID;
        table.Rows.Add(row);
      }
      table.EndLoadData();
      return table;
    }

    private DataTable StringTable(Dictionary<string, string> elements)
    {
      DataTable table = new DataTable();
      table.Columns.Add("Name", typeof(string));
      table.Columns.Add("Value", typeof(string));

      if (elements == null)
        return null;

      table.BeginLoadData();
      foreach (var el in elements)
      {
        DataRow row = table.NewRow();
        row["Name"] = el.Key;
        row["Value"] = el.Value;
        table.Rows.Add(row);
      }
      table.EndLoadData();
      return table;
    }

    public Element ElementByID(string id)
    {
      if (model != null && !string.IsNullOrEmpty(id))
      {
        foreach (Element element in model.Elements)
          if (element.Attributes.ContainsKey("id") &&
            id.Equals(element.Attributes["id"])) return element;
      }
      return null;
    }

  }
}

/*
        public Dictionary<string, string> Attributes;
    public Dictionary<string, List<Element>> Elements;
    public Dictionary<string, List<string>> Properties;
        */
