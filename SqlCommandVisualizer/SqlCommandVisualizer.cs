﻿/******************************************************************************
  Copyright (c) 2011 Seth H. Richards
  
  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:
  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.
  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;


[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(shr.Visualizers.SqlCommandVisualizer.SqlCommandVisualizer),
typeof(shr.Visualizers.SqlCommandVisualizer.SqlCommandObjectSource),
Target = typeof(System.Data.SqlClient.SqlCommand),
Description = "SqlCommand Visualizer")]

namespace shr.Visualizers.SqlCommandVisualizer
{
  public class SqlCommandVisualizer : DialogDebuggerVisualizer
  {
    protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
    {
      String TranslatedSqlText = "";
      using (var ObjReader = new StreamReader(objectProvider.GetData()))
      {
        TranslatedSqlText = ObjReader.ReadToEnd();
      }
      if(String.IsNullOrEmpty(TranslatedSqlText))
        TranslatedSqlText = "[Unable to visualizer provided SqlCommand]";

      
        using (SqlCommandVisualizerForm displayForm = new SqlCommandVisualizerForm())
        {
          displayForm.VisualizationText = TranslatedSqlText;
          windowService.ShowDialog(displayForm);
        }
      

      
    }

    public static void TestShowVisualizer(object objectToVisualize)
    {
      VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, 
        typeof(SqlCommandVisualizer), 
        typeof(SqlCommandObjectSource));
      visualizerHost.ShowVisualizer();
    }
  }
}
