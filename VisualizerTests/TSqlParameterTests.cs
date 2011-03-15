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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using shr.Visualizers.SqlCommandVisualizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace VisualizerTests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class TSqlParameterTests
  {
    public TSqlParameterTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    [TestMethod]
    public void Null_Parameter_Generates_Empty_Declaration()
    {
      SqlParameter Param1 = null;
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("", TSqlParam1.Declaration);
    }

    [TestMethod]
    public void Can_Generate_Int_Declaration()
    {
      SqlParameter Param1 = new SqlParameter("@Param1", System.Data.SqlDbType.Int);
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("DECLARE @Param1 INT;", TSqlParam1.Declaration);
    }

    [TestMethod]
    public void Can_Generate_VARCHAR_No_Size_Declaration()
    {
      SqlParameter Param1 = new SqlParameter("@Param1", System.Data.SqlDbType.VarChar);
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("DECLARE @Param1 VARCHAR(MAX);", TSqlParam1.Declaration);
    }

    [TestMethod]
    public void Can_Generate_VARCHAR_100_Declaration()
    {
      SqlParameter Param1 = new SqlParameter("@Param1", System.Data.SqlDbType.VarChar, 100);
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("DECLARE @Param1 VARCHAR(100);", TSqlParam1.Declaration);
    }

    [TestMethod]
    public void Can_Generate_FLOAT_Declaration()
    {
      SqlParameter Param1 = new SqlParameter("@Param1", System.Data.SqlDbType.Float);
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("DECLARE @Param1 FLOAT;", TSqlParam1.Declaration);
    }

    [TestMethod]
    public void Can_Generate_DECIMAL_18_5_Declaration()
    {
      SqlParameter Param1 = new SqlParameter("@Param1", SqlDbType.Decimal);
      Param1.Precision = 18;
      Param1.Scale = 5;
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("DECLARE @Param1 DECIMAL(18,5);", TSqlParam1.Declaration);
    }

    [TestMethod]
    public void Can_Generate_Int_Assignment()
    {
      SqlParameter Param1 = new SqlParameter("@Param1", System.Data.SqlDbType.Int);
      Param1.Value = 42;
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("SET @Param1 = 42;", TSqlParam1.Assignment);
    }

    [TestMethod]
    public void Should_Not_Generate_Assignment_For_Int_Without_Value()
    {
      SqlParameter Param1 = new SqlParameter("@Param1", System.Data.SqlDbType.Int);
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("", TSqlParam1.Assignment);
    }

    [TestMethod]
    public void Can_Generate_VarChar_Assignment()
    {
      SqlParameter Param1 = new SqlParameter("@Param1", System.Data.SqlDbType.VarChar);
      Param1.Value = "Seth is awesome";
      TSqlParameter TSqlParam1 = new TSqlParameter(Param1);
      Assert.AreEqual("SET @Param1 = 'Seth is awesome';", TSqlParam1.Assignment);
    }

  }
}
