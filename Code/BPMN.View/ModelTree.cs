// 
// The MIT License (MIT)
//
// Copyright (c) 2017 Boris Zinchenko
// mail: info@caseagile.com
// web: http://www.caseagile.com
// code: https://github.com/bzinchenko/bpmnview
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BPMN.View
{
  static class ModelTree
  {
    public static TreeNode AddElementToTree(TreeView tree, TreeNode parent, Element element)
    {
      if (element == null) return null;

      string name = ElementTitle(element);

      TreeNode node = null;
      if (parent == null)
        node = tree.Nodes.Add(name);
      else node = parent.Nodes.Add(name);
      node.Tag = element;

      if (element.Elements != null && element.Elements.Values != null)
      {
        foreach (var entry in element.Elements)
        {
          if (entry.Value != null)
          {
            TreeNode nodeCategory = node.Nodes.Add(entry.Key);
            foreach (Element subElement in entry.Value)
              AddElementToTree(tree, nodeCategory, subElement);
          }
        }
      }
      return node;
    }

    public static TreeNode NodeForElement(TreeNode node, Element element)
    {
      if (node != null)
      {
        if (node.Tag != null && 
          node.Tag == element) return node;

        foreach (TreeNode nd in node.Nodes)
        {
          TreeNode currNode = NodeForElement(nd, element);
          if (currNode != null) return currNode;
        }
      }
      return null;
    }

    public static string ElementTitle(Element element)
    {
      string name = ElementName(element);
      if (string.IsNullOrEmpty(name))
        name = ElementID(element);
      if (string.IsNullOrEmpty(name))
        name = element.TypeName;
      if (string.IsNullOrEmpty(name))
        name = "(unnamed)";
      return name;
    }

    public static string ElementName(Element element)
    {
      if (element != null && element.Attributes.ContainsKey("name"))
        return element.Attributes["name"];
      else return "";
    }

    public static string ElementID(Element element)
    {
      if (element != null && element.Attributes.ContainsKey("id"))
        return element.Attributes["id"];
      else return "";
    }

    public static Element ElementByID(Model model, string id)
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
