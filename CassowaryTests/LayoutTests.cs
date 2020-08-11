using Cassowary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CassowaryTests
{
    [TestClass]
    public class LayoutTests
    {
        #region fields
        private ClVariable _updateTop;
        private ClVariable _updateBottom;
        private ClVariable _updateLeft;
        private ClVariable _updateRight;
        private ClVariable _updateHeight;
        private ClVariable _updateWidth;

        private ClVariable _newpostTop;
        private ClVariable _newpostBottom;
        private ClVariable _newpostLeft;
        private ClVariable _newpostRight;
        private ClVariable _newpostHeight;
        private ClVariable _newpostWidth;

        private ClVariable _quitTop;
        private ClVariable _quitBottom;
        private ClVariable _quitLeft;
        private ClVariable _quitRight;
        private ClVariable _quitHeight;
        private ClVariable _quitWidth;

        private ClVariable _lTitleTop;
        private ClVariable _lTitleBottom;
        private ClVariable _lTitleLeft;
        private ClVariable _lTitleRight;
        private ClVariable _lTitleHeight;
        private ClVariable _lTitleWidth;

        private ClVariable _titleTop;
        private ClVariable _titleBottom;
        private ClVariable _titleLeft;
        private ClVariable _titleRight;
        private ClVariable _titleHeight;
        private ClVariable _titleWidth;

        private ClVariable _lBodyTop;
        private ClVariable _lBodyBottom;
        private ClVariable _lBodyLeft;
        private ClVariable _lBodyRight;
        private ClVariable _lBodyHeight;
        private ClVariable _lBodyWidth;

        private ClVariable _blogentryTop;
        private ClVariable _blogentryBottom;
        private ClVariable _blogentryLeft;
        private ClVariable _blogentryRight;
        private ClVariable _blogentryHeight;
        private ClVariable _blogentryWidth;

        private ClVariable _lRecentTop;
        private ClVariable _lRecentBottom;
        private ClVariable _lRecentLeft;
        private ClVariable _lRecentRight;
        private ClVariable _lRecentHeight;
        private ClVariable _lRecentWidth;

        private ClVariable _articlesTop;
        private ClVariable _articlesBottom;
        private ClVariable _articlesLeft;
        private ClVariable _articlesRight;
        private ClVariable _articlesHeight;
        private ClVariable _articlesWidth;

        ////////////////////////////////////////////////////////////////
        //                  Container widgets                         // 
        ////////////////////////////////////////////////////////////////

        private ClVariable _topRightTop;
        private ClVariable _topRightBottom;
        private ClVariable _topRightLeft;
        private ClVariable _topRightRight;
        private ClVariable _topRightHeight;
        private ClVariable _topRightWidth;

        private ClVariable _bottomRightTop;
        private ClVariable _bottomRightBottom;
        private ClVariable _bottomRightLeft;
        private ClVariable _bottomRightRight;
        private ClVariable _bottomRightHeight;
        private ClVariable _bottomRightWidth;

        private ClVariable _rightTop;
        private ClVariable _rightBottom;
        private ClVariable _rightLeft;
        private ClVariable _rightRight;
        private ClVariable _rightHeight;
        private ClVariable _rightWidth;

        private ClVariable _leftTop;
        private ClVariable _leftBottom;
        private ClVariable _leftLeft;
        private ClVariable _leftRight;
        private ClVariable _leftHeight;
        private ClVariable _leftWidth;

        private ClVariable _frTop;
        private ClVariable _frBottom;
        private ClVariable _frLeft;
        private ClVariable _frRight;
        private ClVariable _frHeight;
        private ClVariable _frWidth;
        #endregion

        [TestMethod]
        public void TestLayout1()
        {
            var solver = new ClSimplexSolver();

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

            _updateTop = new ClVariable("update.top", 0);
            _updateBottom = new ClVariable("update.bottom", 23);
            _updateLeft = new ClVariable("update.left", 0);
            _updateRight = new ClVariable("update.right", 75);
            _updateHeight = new ClVariable("update.height", 23);
            _updateWidth = new ClVariable("update.width", 75);

            _newpostTop = new ClVariable("newpost.top", 0);
            _newpostBottom = new ClVariable("newpost.bottom", 23);
            _newpostLeft = new ClVariable("newpost.left", 0);
            _newpostRight = new ClVariable("newpost.right", 75);
            _newpostWidth = new ClVariable("newpost.width", 75);
            _newpostHeight = new ClVariable("newpost.height", 23);

            _quitBottom = new ClVariable("quit.bottom", 23);
            _quitRight = new ClVariable("quit.right", 75);
            _quitHeight = new ClVariable("quit.height", 23);
            _quitWidth = new ClVariable("quit.width", 75);
            _quitLeft = new ClVariable("quit.left", 0);
            _quitTop = new ClVariable("quit.top", 0);

            _lTitleTop = new ClVariable("l_title.top", 0);
            _lTitleBottom = new ClVariable("l_title.bottom", 23);
            _lTitleLeft = new ClVariable("l_title.left", 0);
            _lTitleRight = new ClVariable("l_title.right", 100);
            _lTitleHeight = new ClVariable("l_title.height", 23);
            _lTitleWidth = new ClVariable("l_title.width", 100);

            _titleTop = new ClVariable("title.top", 0);
            _titleBottom = new ClVariable("title.bottom", 20);
            _titleLeft = new ClVariable("title.left.", 0);
            _titleRight = new ClVariable("title.right", 100);
            _titleHeight = new ClVariable("title.height", 20);
            _titleWidth = new ClVariable("title.width", 100);

            _lBodyTop = new ClVariable("l_body.top", 0);
            _lBodyBottom = new ClVariable("l_body.bottom", 23);
            _lBodyLeft = new ClVariable("l_body.left", 0);
            _lBodyRight = new ClVariable("l_body.right", 100);
            _lBodyHeight = new ClVariable("l_body.height.", 23);
            _lBodyWidth = new ClVariable("l_body.width", 100);

            _blogentryTop = new ClVariable("blogentry.top", 0);
            _blogentryBottom = new ClVariable("blogentry.bottom", 315);
            _blogentryLeft = new ClVariable("blogentry.left", 0);
            _blogentryRight = new ClVariable("blogentry.right", 400);
            _blogentryHeight = new ClVariable("blogentry.height", 315);
            _blogentryWidth = new ClVariable("blogentry.width", 400);


            _lRecentTop = new ClVariable("l_recent.top", 0);
            _lRecentBottom = new ClVariable("l_recent.bottom", 23);
            _lRecentLeft = new ClVariable("l_recent.left", 0);
            _lRecentRight = new ClVariable("l_recent.right", 100);
            _lRecentHeight = new ClVariable("l_recent.height", 23);
            _lRecentWidth = new ClVariable("l_recent.width", 100);

            _articlesTop = new ClVariable("articles.top", 0);
            _articlesBottom = new ClVariable("articles.bottom", 415);
            _articlesLeft = new ClVariable("articles.left", 0);
            _articlesRight = new ClVariable("articles.right", 180);
            _articlesHeight = new ClVariable("articles.height", 415);
            _articlesWidth = new ClVariable("articles.width", 100);

            ////////////////////////////////////////////////////////////////
            //                  Container widgets                         // 
            ////////////////////////////////////////////////////////////////

            _topRightTop = new ClVariable("topRight.top", 0);
            _topRightBottom = new ClVariable("topRight.bottom", 100);
            _topRightLeft = new ClVariable("topRight.left", 0);
            _topRightRight = new ClVariable("topRight.right", 200);
            _topRightHeight = new ClVariable("topRight.height", 100);
            _topRightWidth = new ClVariable("topRight.width", 200);

            _bottomRightTop = new ClVariable("bottomRight.top", 0);
            _bottomRightBottom = new ClVariable("bottomRight.bottom", 100);
            _bottomRightLeft = new ClVariable("bottomRight.left", 0);
            _bottomRightRight = new ClVariable("bottomRight.right", 200);
            _bottomRightHeight = new ClVariable("bottomRight.height", 100);
            _bottomRightWidth = new ClVariable("bottomRight.width", 200);

            _rightTop = new ClVariable("right.top", 0);
            _rightBottom = new ClVariable("right.bottom", 100);
            _rightLeft = new ClVariable("right.left", 0);
            _rightRight = new ClVariable("right.right", 200);
            _rightHeight = new ClVariable("right.height", 100);
            _rightWidth = new ClVariable("right.width", 200);

            _leftTop = new ClVariable("left.top", 0);
            _leftBottom = new ClVariable("left.bottom", 100);
            _leftLeft = new ClVariable("left.left", 0);
            _leftRight = new ClVariable("left.right", 200);
            _leftHeight = new ClVariable("left.height", 100);
            _leftWidth = new ClVariable("left.width", 200);

            _frTop = new ClVariable("fr.top", 0);
            _frBottom = new ClVariable("fr.bottom", 100);
            _frLeft = new ClVariable("fr.left", 0);
            _frRight = new ClVariable("fr.right", 200);
            _frHeight = new ClVariable("fr.height", 100);
            _frWidth = new ClVariable("fr.width", 200);
        }

        private void BuildConstraints(ClSimplexSolver solver)
        {
            BuildStayConstraints(solver);
            BuildRequiredConstraints(solver);
            BuildStrongConstraints(solver);
        }

        private void BuildStayConstraints(ClSimplexSolver solver)
        {
            solver.AddStay(_updateHeight);
            solver.AddStay(_updateWidth);
            solver.AddStay(_newpostHeight);
            solver.AddStay(_newpostWidth);
            solver.AddStay(_quitHeight);
            solver.AddStay(_quitWidth);
            solver.AddStay(_lTitleHeight);
            solver.AddStay(_lTitleWidth);
            solver.AddStay(_titleHeight);
            solver.AddStay(_titleWidth);
            solver.AddStay(_lBodyHeight);
            solver.AddStay(_lBodyWidth);
            solver.AddStay(_blogentryHeight);
            // let's keep blogentry.width in favor of other stay constraints!
            // remember we later specify title.width to be equal to blogentry.width
            solver.AddStay(_blogentryWidth, ClStrength.Strong);
            solver.AddStay(_lRecentHeight);
            solver.AddStay(_lRecentWidth);
            solver.AddStay(_articlesHeight);
            solver.AddStay(_articlesWidth);
        }

        private void BuildRequiredConstraints(ClSimplexSolver solver)
        {
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_bottomRightHeight), _bottomRightTop), new ClLinearExpression(_bottomRightBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_bottomRightWidth), _bottomRightLeft), new ClLinearExpression(_bottomRightRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_bottomRightTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_bottomRightBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_bottomRightLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_bottomRightRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_updateHeight), _updateTop), new ClLinearExpression(_updateBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_updateWidth), _updateLeft), new ClLinearExpression(_updateRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_updateTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_updateBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_updateLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_updateRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_updateRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_updateBottom, Cl.Operator.LessThanOrEqualTo, _bottomRightHeight));
            solver.AddConstraint(new ClLinearInequality(_updateRight, Cl.Operator.LessThanOrEqualTo, _bottomRightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_newpostHeight), _newpostTop), new ClLinearExpression(_newpostBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_newpostWidth), _newpostLeft), new ClLinearExpression(_newpostRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_newpostTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_newpostBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_newpostLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_newpostRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_newpostRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_newpostBottom, Cl.Operator.LessThanOrEqualTo, _bottomRightHeight));
            solver.AddConstraint(new ClLinearInequality(_newpostRight, Cl.Operator.LessThanOrEqualTo, _bottomRightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_quitHeight), _quitTop), new ClLinearExpression(_quitBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_quitWidth), _quitLeft), new ClLinearExpression(_quitRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_quitTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_quitBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_quitLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_quitRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_quitRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_quitBottom, Cl.Operator.LessThanOrEqualTo, _bottomRightHeight));
            solver.AddConstraint(new ClLinearInequality(_quitRight, Cl.Operator.LessThanOrEqualTo, _bottomRightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_topRightHeight), _topRightTop), new ClLinearExpression(_topRightBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_topRightWidth), _topRightLeft), new ClLinearExpression(_topRightRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_topRightTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_topRightBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_topRightLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_topRightRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_topRightRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_lTitleHeight), _lTitleTop), new ClLinearExpression(_lTitleBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_lTitleWidth), _lTitleLeft), new ClLinearExpression(_lTitleRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lTitleTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lTitleBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lTitleLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lTitleRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lTitleRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lTitleBottom, Cl.Operator.LessThanOrEqualTo, _topRightHeight));
            solver.AddConstraint(new ClLinearInequality(_lTitleRight, Cl.Operator.LessThanOrEqualTo, _topRightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_titleHeight), _titleTop), new ClLinearExpression(_titleBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_titleWidth), _titleLeft), new ClLinearExpression(_titleRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_titleTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_titleBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_titleLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_titleRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_titleRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_titleBottom, Cl.Operator.LessThanOrEqualTo, _topRightHeight));
            solver.AddConstraint(new ClLinearInequality(_titleRight, Cl.Operator.LessThanOrEqualTo, _topRightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_lBodyHeight), _lBodyTop), new ClLinearExpression(_lBodyBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_lBodyWidth), _lBodyLeft), new ClLinearExpression(_lBodyRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lBodyTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lBodyBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lBodyLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lBodyRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lBodyRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lBodyBottom, Cl.Operator.LessThanOrEqualTo, _topRightHeight));
            solver.AddConstraint(new ClLinearInequality(_lBodyRight, Cl.Operator.LessThanOrEqualTo, _topRightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_blogentryHeight), _blogentryTop), new ClLinearExpression(_blogentryBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_blogentryWidth), _blogentryLeft), new ClLinearExpression(_blogentryRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_blogentryTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_blogentryBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_blogentryLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_blogentryRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_blogentryRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_blogentryBottom, Cl.Operator.LessThanOrEqualTo, _topRightHeight));
            solver.AddConstraint(new ClLinearInequality(_blogentryRight, Cl.Operator.LessThanOrEqualTo, _topRightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_leftHeight), _leftTop), new ClLinearExpression(_leftBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_leftWidth), _leftLeft), new ClLinearExpression(_leftRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_lRecentHeight), _lRecentTop), new ClLinearExpression(_lRecentBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_lRecentWidth), _lRecentLeft), new ClLinearExpression(_lRecentRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lRecentTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lRecentBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lRecentLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lRecentRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lRecentRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_lRecentBottom, Cl.Operator.LessThanOrEqualTo, _leftHeight));
            solver.AddConstraint(new ClLinearInequality(_lRecentRight, Cl.Operator.LessThanOrEqualTo, _leftWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_articlesHeight), _articlesTop), new ClLinearExpression(_articlesBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_articlesWidth), _articlesLeft), new ClLinearExpression(_articlesRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_articlesTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_articlesBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_articlesLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_articlesRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_articlesRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_articlesBottom, Cl.Operator.LessThanOrEqualTo, _leftHeight));
            solver.AddConstraint(new ClLinearInequality(_articlesRight, Cl.Operator.LessThanOrEqualTo, _leftWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_leftHeight), _leftTop), new ClLinearExpression(_leftBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_leftWidth), _leftLeft), new ClLinearExpression(_leftRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_leftRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_rightHeight), _rightTop), new ClLinearExpression(_rightBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_rightWidth), _rightLeft), new ClLinearExpression(_rightRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_rightTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_rightBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_rightLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_rightRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_rightRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));

            solver.AddConstraint(new ClLinearInequality(_topRightBottom, Cl.Operator.LessThanOrEqualTo, _rightHeight));
            solver.AddConstraint(new ClLinearInequality(_topRightRight, Cl.Operator.LessThanOrEqualTo, _rightWidth));

            solver.AddConstraint(new ClLinearInequality(_bottomRightBottom, Cl.Operator.LessThanOrEqualTo, _rightHeight));
            solver.AddConstraint(new ClLinearInequality(_bottomRightRight, Cl.Operator.LessThanOrEqualTo, _rightWidth));

            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_frHeight), _frTop), new ClLinearExpression(_frBottom), ClStrength.Required));
            solver.AddConstraint(new ClLinearEquation(Cl.Plus(new ClLinearExpression(_frWidth), _frLeft), new ClLinearExpression(_frRight), ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_frTop, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_frBottom, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_frLeft, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_frRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));
            solver.AddConstraint(new ClLinearInequality(_frRight, Cl.Operator.GreaterThanOrEqualTo, 0, ClStrength.Required));

            solver.AddConstraint(new ClLinearInequality(_leftBottom, Cl.Operator.LessThanOrEqualTo, _frHeight));
            solver.AddConstraint(new ClLinearInequality(_leftRight, Cl.Operator.LessThanOrEqualTo, _frWidth));
            solver.AddConstraint(new ClLinearInequality(_rightBottom, Cl.Operator.LessThanOrEqualTo, _frHeight));
            solver.AddConstraint(new ClLinearInequality(_rightRight, Cl.Operator.LessThanOrEqualTo, _frWidth));
        }

        private void BuildStrongConstraints(ClSimplexSolver solver)
        {
            solver.AddConstraint(new ClLinearInequality(_updateRight, Cl.Operator.LessThanOrEqualTo, _newpostLeft, ClStrength.Strong));
            solver.AddConstraint(new ClLinearInequality(_newpostRight, Cl.Operator.LessThanOrEqualTo, _quitLeft, ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(bottomRight_width, new ClLinearExpression(topRight_width), ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(right_width, new ClLinearExpression(topRight_width), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_bottomRightBottom, new ClLinearExpression(_rightBottom), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_newpostHeight, new ClLinearExpression(_updateHeight), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_newpostWidth, new ClLinearExpression(_updateWidth), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_updateHeight, new ClLinearExpression(_quitHeight), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_quitWidth, new ClLinearExpression(_updateWidth), ClStrength.Strong));

            solver.AddConstraint(new ClLinearInequality(_lTitleBottom, Cl.Operator.LessThanOrEqualTo, _titleTop, ClStrength.Strong));
            solver.AddConstraint(new ClLinearInequality(_titleBottom, Cl.Operator.LessThanOrEqualTo, _lBodyTop, ClStrength.Strong));
            solver.AddConstraint(new ClLinearInequality(_lBodyBottom, Cl.Operator.LessThanOrEqualTo, _blogentryTop, ClStrength.Strong));

            solver.AddConstraint(new ClLinearEquation(_titleWidth, new ClLinearExpression(_blogentryWidth), ClStrength.Strong));

            solver.AddConstraint(new ClLinearInequality(_lRecentBottom, Cl.Operator.LessThanOrEqualTo, _articlesTop, ClStrength.Strong));

            solver.AddConstraint(new ClLinearInequality(_topRightBottom, Cl.Operator.LessThanOrEqualTo, _bottomRightTop, ClStrength.Strong));
            solver.AddConstraint(new ClLinearInequality(_leftRight, Cl.Operator.LessThanOrEqualTo, _rightLeft, ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(left_height, new ClLinearExpression(right_height), ClStrength.Strong));
            //_solver.AddConstraint(new ClLinearEquation(fr_height, new ClLinearExpression(right_height), ClStrength.Strong));

            // alignment
            solver.AddConstraint(new ClLinearEquation(_lTitleLeft, new ClLinearExpression(_titleLeft), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_titleLeft, new ClLinearExpression(_blogentryLeft), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_lBodyLeft, new ClLinearExpression(_blogentryLeft), ClStrength.Strong));
            solver.AddConstraint(new ClLinearEquation(_lRecentLeft, new ClLinearExpression(_articlesLeft), ClStrength.Strong));
        }
    }
}
