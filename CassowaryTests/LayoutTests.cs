using Cassowary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CassowaryTests
{
    [TestClass]
    public class LayoutTests
    {
        #region fields
        private ClVariable _updateLeft;
        private ClVariable _updateRight;
        private ClVariable _updateWidth;

        private ClVariable _newpostLeft;
        private ClVariable _newpostRight;
        private ClVariable _newpostWidth;

        private ClVariable _quitLeft;
        private ClVariable _quitRight;
        private ClVariable _quitWidth;

  
        private ClVariable _titleLeft;
        private ClVariable _titleRight;
        private ClVariable _titleWidth;

        private ClVariable _blogentryLeft;
        private ClVariable _blogentryRight;
        private ClVariable _blogentryWidth;

      //  private ClVariable _articlesLeft;
      //  private ClVariable _articlesRight;

        ////////////////////////////////////////////////////////////////
        //                  Container widgets                         // 
        ////////////////////////////////////////////////////////////////

        private ClVariable _topRightLeft;
        private ClVariable _topRightRight;
        private ClVariable _topRightWidth;

        private ClVariable _bottomRightLeft;
        private ClVariable _bottomRightRight;
        private ClVariable _bottomRightWidth;

        private ClVariable _rightWidth;

        #endregion

        [TestMethod]
        public void TestLayout1()
        {
            var solver = new ClSimplexSolver();
           // solver.AutoSolve = false;

            // initialize the needed variables
            BuildVariables();
            BuildConstraints(solver);

            solver.Solve();
        }

        private void BuildVariables()
        {
            ////////////////////////////////////////////////////////////////
            //                 Individual widgets                         // 
            ////////////////////////////////////////////////////////////////

            _updateLeft = new ClVariable("update.left", 0);
            _updateRight = new ClVariable("update.right", 75);
            _updateWidth = new ClVariable("update.width", 75);

            _newpostLeft = new ClVariable("newpost.left", 0);
            _newpostRight = new ClVariable("newpost.right", 75);
            _newpostWidth = new ClVariable("newpost.width", 75);

            _quitRight = new ClVariable("quit.right", 75);
            _quitWidth = new ClVariable("quit.width", 75);
            _quitLeft = new ClVariable("quit.left", 0);

            _titleLeft = new ClVariable("title.left.", 0);
            _titleRight = new ClVariable("title.right", 100);
            _titleWidth = new ClVariable("title.width", 100);

           
            _blogentryLeft = new ClVariable("blogentry.left", 0);
            _blogentryRight = new ClVariable("blogentry.right", 400);
            _blogentryWidth = new ClVariable("blogentry.width", 400);


            _topRightLeft = new ClVariable("topRight.left", 0);
            _topRightRight = new ClVariable("topRight.right", 200);
            _topRightWidth = new ClVariable("topRight.width", 200);

            _bottomRightLeft = new ClVariable("bottomRight.left", 0);
            _bottomRightRight = new ClVariable("bottomRight.right", 200);
            _bottomRightWidth = new ClVariable("bottomRight.width", 200);

            _rightWidth = new ClVariable("right.width", 200);
        }

        private void BuildConstraints(ClSimplexSolver solver)
        {
            BuildStayConstraints(solver);
            BuildRequiredConstraints(solver);
            BuildStrongConstraints(solver);
        }

        private void BuildStayConstraints(ClSimplexSolver solver)
        {
            solver.AddStay(_quitWidth);
            solver.AddStay(_blogentryWidth, ClStrength.Strong);
        }

        private void BuildRequiredConstraints(ClSimplexSolver solver)
        {
     /**/   solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_bottomRightWidth), _bottomRightLeft), new ClLinearExpression(_bottomRightRight), ClStrength.Required));
     /**/   solver.AddConstraint(new ClLinearInequality(_bottomRightLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
  
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_updateWidth), _updateLeft), new ClLinearExpression(_updateRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_updateLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_newpostWidth), _newpostLeft), new ClLinearExpression(_newpostRight), ClStrength.Required));
    
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_quitWidth), _quitLeft), new ClLinearExpression(_quitRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_quitRight, Cl.Operator.LessThanOrEqualTo, _bottomRightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_topRightWidth), _topRightLeft), new ClLinearExpression(_topRightRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_topRightLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
         
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_titleWidth), _titleLeft), new ClLinearExpression(_titleRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_titleRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_blogentryWidth), _blogentryLeft), new ClLinearExpression(_blogentryRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_blogentryLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_blogentryRight, Cl.Operator.LessThanOrEqualTo, _topRightWidth));


            solver.AddConstraint(new ClLinearInequality(_topRightRight, Cl.Operator.LessThanOrEqualTo, _rightWidth));

            solver.AddConstraint(new ClLinearInequality(_bottomRightRight, Cl.Operator.LessThanOrEqualTo, _rightWidth));

        }

        private void BuildStrongConstraints(ClSimplexSolver solver)
        {
            solver.AddConstraint(new ClLinearInequality(_updateRight, Cl.Operator.LessThanOrEqualTo, _newpostLeft, ClStrength.Strong));
            solver.AddConstraint(new ClLinearInequality(_newpostRight, Cl.Operator.LessThanOrEqualTo, _quitLeft, ClStrength.Strong));
              solver.AddConstraint(new ClLinearEquation(_newpostWidth, new ClLinearExpression(_updateWidth), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_quitWidth, new ClLinearExpression(_updateWidth), ClStrength.Strong));

            solver.AddConstraint(new ClLinearEquation(_titleWidth, new ClLinearExpression(_blogentryWidth), ClStrength.Strong));

            solver.AddConstraint(new ClLinearEquation(_titleLeft, new ClLinearExpression(_blogentryLeft), ClStrength.Strong));
        }
    }
}
