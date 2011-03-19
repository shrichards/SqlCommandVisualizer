/******************************************************************************
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
using System.Data.SqlClient;
namespace shr.Visualizers.SqlCommandVisualizer
{

  public class TSqlGenerator
  {
    protected SqlCommand m_Command = null;
    protected List<String> _Declarations = new List<string>();
    protected List<String> _Assignments = new List<string>();
    protected String _Translation;

    public TSqlGenerator(SqlCommand Cmd)
    {
      m_Command = Cmd;

      List<TSqlParameter> TSqlParams = new List<TSqlParameter>();
      foreach (SqlParameter param in Cmd.Parameters)
      {
        TSqlParameter TSql = new TSqlParameter(param);
        _Declarations.Add(String.Format("{0}", TSql.Declaration));

        if(!String.IsNullOrEmpty(TSql.Assignment))
          _Assignments.Add(String.Format("{0}", TSql.Assignment));
      }

    }

    public List<String> Declarations
    {
      get { return _Declarations; }
    }

    public List<String> Assignments
    {
      get { return _Assignments; }
    }

    public String TextTranslation
    {
      get 
      {
        StringBuilder Translation = new StringBuilder();
        _Declarations.ForEach(decl => Translation.AppendFormat("{0}{1}", decl, Environment.NewLine));
        _Assignments.ForEach(assg => Translation.AppendFormat("{0}{1}", assg, Environment.NewLine));
        return Translation.ToString();
      }
    }
  }
}
