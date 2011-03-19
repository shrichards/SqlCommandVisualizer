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
using System.Data;
using System.Data.SqlClient;

namespace shr.Visualizers.SqlCommandVisualizer
{
  public class TSqlParameter
  {
    protected SqlParameter _Parameter = null;
    protected readonly String _Declaration = "";
    protected readonly String _Assignment = "";

    public TSqlParameter(SqlParameter param1)
    {
      Dictionary<SqlDbType, Func<SqlParameter, String>> Generators = new Dictionary<SqlDbType, Func<SqlParameter, string>>
      {
        {SqlDbType.VarChar, VarCharDeclaration},
        {SqlDbType.NVarChar, NVarCharDeclaration},
        {SqlDbType.Decimal, DecimalDeclaration}
      };

      Dictionary<SqlDbType, Func<SqlParameter, String>> Assignors = new Dictionary<SqlDbType, Func<SqlParameter, string>>
      {
        {SqlDbType.VarChar, TextAssignment},
        {SqlDbType.NVarChar, TextAssignment},
        {SqlDbType.Int, NumericAssigment}
      };

      _Parameter = param1;

      if (_Parameter == null)
        _Declaration = "";
      else if (Generators.ContainsKey(_Parameter.SqlDbType))
        _Declaration = Generators[_Parameter.SqlDbType](_Parameter);
      else
        _Declaration = DefaultDeclaration(_Parameter);

      if (_Parameter == null)
        _Assignment = "";
      else if (Assignors.ContainsKey(_Parameter.SqlDbType) && _Parameter.Value != null)
        _Assignment = Assignors[_Parameter.SqlDbType](_Parameter);
      else
        _Assignment = "";
        
    }

    public static String VarCharDeclaration(SqlParameter Param)
    {
      String Size;
      if (Param.Size > 0)
        Size = Param.Size.ToString();
      else
        Size = "MAX";
      return String.Format("DECLARE {0} VARCHAR({1});", Param.ParameterName, Size);
    }
    public static String NVarCharDeclaration(SqlParameter Param)
    {
      String Size;
      if (Param.Size > 0)
        Size = Param.Size.ToString();
      else
        Size = "MAX";
      return String.Format("DECLARE {0} NVARCHAR({1});", Param.ParameterName, Size);
    }
    public static String DecimalDeclaration(SqlParameter Param)
    {
      String Precision = Param.Precision.ToString();
      String Scale = Param.Scale.ToString();      
      
      return String.Format("DECLARE {0} DECIMAL({1},{2});", Param.ParameterName, Precision, Scale);
    }
    public static String DefaultDeclaration(SqlParameter Param)
    {

      String TypeName = Enum.GetName(typeof(SqlDbType), Param.SqlDbType).ToUpper();
      return String.Format("DECLARE {0} {1};", Param.ParameterName, TypeName);
    }

    public static String NumericAssigment(SqlParameter Param)
    {
      return String.Format("SET {0} = {1};", Param.ParameterName, Param.Value);
    }
    public static String TextAssignment(SqlParameter Param)
    {
      return String.Format("SET {0} = '{1}';", Param.ParameterName, Param.Value);
    }

    public String Declaration
    {
      get { return _Declaration; }
    }
    public String Assignment
    {
      get { return _Assignment; }
    }
  }
}
