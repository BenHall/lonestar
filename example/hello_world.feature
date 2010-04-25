Feature: Meerkatalyst.Lonestar Hello World!
		 Visit http://www.meerkatalyst.com
		 and http://blog.benhall.me.uk

 Scenario: Go Green
   Given a scenario
   When it passes
   Then the steps should go green
   
 Scenario: Go Yellow
   Given a scenario
   When the "then" step is not implemented
   Then it should go yellow
   
 Scenario: Go Red
   Given a scenario
   When "then" fails
   Then it should go red