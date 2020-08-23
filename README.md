## Cassowary.net README

Cassowary.net [1] is a port of the Cassowary constraint solving toolkit [2] to the .NET platform. It is based on the Java version by Greg J. Badros, which in turn is based on the Smalltalk version by Alan Borning.

Fisrt fork of Cassowary.net initially began as a simple cleanup effort, refactoring the ported Java style code to a C# style of code. A new expression based constraint composition system has been added.

Current fork does replaces expression based constraint syntax with C# operator overrides and optimises it for GUI use.

## Composing Constraints

Creating complex constraints is very easy using the expression based extensions to the system:

    var solver = new ClSimplexSolver();
    
	var a = new ClVariable("a");
    solver.AddConstraint(a > 10);
    
This basic expression will create a constraint which operates on one variable, named "a", and adds a constraint that "a" must be greater than 10. Much more complex constraints than this can be created:

	var bl = new ClVariable("bl");
	var br = new ClVariable("br");
	var wl = new ClVariable("wl");
	var wr = new ClVariable("wr");
	
    solver.AddConstraints(bl ^ wl, br ^ wr, br > 100);
    
This approach using C# operator overrides as constraint builder
	
Also, you can create constraints directly from expressions: 
	var a = new ClVariable("a");
    
    var cn = new ClLinearInequality(new ClLinearExpression(a), Cl.Operator.GreaterThan, 10.0);
    solver.AddConstraint(cn);

	
	