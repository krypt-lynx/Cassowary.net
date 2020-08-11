## Cassowary.net README

Cassowary.net [1] is a port of the Cassowary constraint solving toolkit [2] to the .NET platform. It is based on the Java version by Greg J. Badros, which in turn is based on the Smalltalk version by Alan Borning.

This fork of Cassowary.net initially began as a simple cleanup effort, refactoring the ported Java style code to a C# style of code. A new expression based constraint composition system has been added.

## Nuget

The easiest way to use this fork is from the [nuget](https://www.nuget.org/packages/Cassowary/) package manager.

## Status

I am not personally actively using this at the moment. I will accept PRs but I am unlikely to dedicate much time to fixing bugs. **I would recommend using the *far* more powerful [Z3](https://github.com/Z3Prover/z3) solver built by Microsoft instead**. In my tests Z3 has shown itself to be generally faster than Cassowary at solving linear problems, whilst having the capacity to solve non-linear problems when required.

## Composing Constraints

Creating complex constraints is very easy using the expression based extensions to the system:

    var solver = new ClSimplexSolver();
    
    solver.AddConstraint(a => a > 10);
    
This basic expression will create a constraint which operates on one variable, named "a", and adds a constraint that "a" must be greater than 10. Much more complex constraints than this can be created:

    solver.AddConstraint((bl, br, wl, wr) => bl == wl && br == wr || br > 100);
    
Of course, it may not be convenient to include your variable names directly in your C# variable definitions, the above example is difficult to read because we have no idea what the definitions means. However if we rewrite it with meaningful names...

    solver.AddConstraint((buttonLeft, buttonRight, windowLeft, windowRight) => buttonLeft == windowLeft && buttonRight == windowRight || buttonRight > 100);
    
...it becomes very long and tiresome. To solve this problem you can pass in your variable objects directly, and then bind them to temporary names:

    var buttonLeft = new ClVariable("buttonLeft");
    var buttonRight = new ClVariable("buttonRight");
    var windowLeft = new ClVariable("windowLeft");
    var windowRight = new ClVariable("windowRight");
    
    solver.AddConstraint(
        buttonLeft, buttonRight, windowLeft, windowRight        // Pass in the variables here
        (bl, br, wl, wr) =>                                     // Bind them to temporary names in this expression
        bl == wl && br == wr || br > 100);                  // Define the constraints, using the temporary names
    );
