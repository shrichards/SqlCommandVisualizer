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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using shr.Visualizers.SqlCommandVisualizer;
using System.Data;

namespace VisualizerTests
{
  /// <summary>
  /// Summary description for TSqlGeneratorTests
  /// </summary>
  [TestClass]
  public class TSqlGeneratorTests
  {
    public TSqlGeneratorTests()
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

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    public void Can_Generate_SQL_For_Single_Parameter()
    {
      SqlCommand Cmd = new SqlCommand();
      Cmd.Parameters.AddWithValue("@IntParam", 33);
      TSqlGenerator Generator = new TSqlGenerator(Cmd);

      Assert.AreEqual(1, Generator.Declarations.Count);
      Assert.AreEqual(1, Generator.Assignments.Count);
    }

    [TestMethod]
    public void Should_Only_Generate_Declaration_For_Param_Without_Value()
    {
      using (SqlCommand Cmd = new SqlCommand())
      {
        Cmd.Parameters.Add("@Param", SqlDbType.Int);

        TSqlGenerator Generator = new TSqlGenerator(Cmd);
        Assert.AreEqual(1, Generator.Declarations.Count);
        Assert.AreEqual(0, Generator.Assignments.Count);
      }
    }

    [TestMethod]
    public void Should_Generate_Declaration_And_Assignment_For_Param_Witht_Value()
    {
      using (SqlCommand Cmd = new SqlCommand())
      {
        Cmd.Parameters.AddWithValue("@Param", "The Parameter");

        TSqlGenerator Generator = new TSqlGenerator(Cmd);
        Assert.AreEqual(1, Generator.Declarations.Count);
        Assert.AreEqual(1, Generator.Assignments.Count);
      }
    }

    [TestMethod]
    public void Should_Include_Declarations_And_Assignments_In_Translation()
    {
      using (SqlCommand Cmd = new SqlCommand())
      {
        Cmd.Parameters.AddWithValue("@Param", "The Parameter");

        TSqlParameter TheParameter = new TSqlParameter(Cmd.Parameters[0]);

        TSqlGenerator Generator = new TSqlGenerator(Cmd);
        String GeneratedTSql = Generator.TextTranslation;

        Assert.IsTrue(GeneratedTSql.Contains(TheParameter.Declaration));
        Assert.IsTrue(GeneratedTSql.Contains(TheParameter.Assignment));
      }
    }

    [TestMethod]
    public void Should_Output_Declaration_Before_Assignment_In_Translation()
    {
      using (SqlCommand Cmd = new SqlCommand())
      {
        Cmd.Parameters.AddWithValue("@Param", "The Parameter");

        TSqlParameter TheParameter = new TSqlParameter(Cmd.Parameters[0]);

        TSqlGenerator Generator = new TSqlGenerator(Cmd);
        String GeneratedTSql = Generator.TextTranslation;
        
        int DeclarationIndex = GeneratedTSql.IndexOf(TheParameter.Declaration);
        int AssignmentIndex = GeneratedTSql.IndexOf(TheParameter.Assignment);

        Assert.IsTrue(DeclarationIndex < AssignmentIndex, "Declaration does not appear before assignment");
      }
    }
    
    [TestMethod]
    public void Should_Break_Before_FROM()
    {
      using (SqlCommand Cmd = new SqlCommand("SELECT * FROM the_table"))
      {
        TSqlGenerator Generator = new TSqlGenerator(Cmd);
        String GeneratedTSql = Generator.TextTranslation;
        String MatchText = String.Format("{0}FROM", Environment.NewLine);
        Assert.IsTrue(GeneratedTSql.Contains(MatchText));
      }
    }

    [TestMethod]
    public void Should_Break_Before_WHERE()
    {
      using (SqlCommand Cmd = new SqlCommand("SELECT * FROM the_table WHERE val = 1"))
      {
        TSqlGenerator Generator = new TSqlGenerator(Cmd);
        String GeneratedTSql = Generator.TextTranslation;
        String MatchText = String.Format("{0}WHERE", Environment.NewLine);
        Assert.IsTrue(GeneratedTSql.Contains(MatchText));
      }
    }

    [TestMethod]
    public void Should_Break_Before_ORDER()
    {
      using (SqlCommand Cmd = new SqlCommand("SELECT * FROM the_table ORDER BY the_col"))
      {
        TSqlGenerator Generator = new TSqlGenerator(Cmd);
        String GeneratedTSql = Generator.TextTranslation;
        String MatchText = String.Format("{0}ORDER", Environment.NewLine);
        Assert.IsTrue(GeneratedTSql.Contains(MatchText));
      }
    }

    [TestMethod]
    public void Should_Break_Before_GROUP()
    {
      using (SqlCommand Cmd = new SqlCommand("SELECT * FROM the_table GROUP BY the_col"))
      {
        TSqlGenerator Generator = new TSqlGenerator(Cmd);
        String GeneratedTSql = Generator.TextTranslation;
        String MatchText = String.Format("{0}GROUP", Environment.NewLine);
        Assert.IsTrue(GeneratedTSql.Contains(MatchText));
      }
    }

  }

}
