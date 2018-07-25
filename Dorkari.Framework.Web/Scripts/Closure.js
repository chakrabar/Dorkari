/* Functions in JavaScript form closures. Specifically, when a function creates an inner function.
A closure is the combination of a function and the lexical environment within which that function was declared. 
This environment consists of any local variables that were in-scope at the time the closure was created. 
Note 1: Inner functions have access to the variables of outer functions (closure), and global ones
Note 2: Variables defined without `var` are always global, even when declared within a function */

/* Basically, inner functions remember variables in it's outer scope, and has access to it's current value!
It creates a function with own variable, just like a class in OOP with property and a single function */

function makeFunc() {
  var name = 'Mozilla';
  function displayName() {
    alert(name);
  }
  return displayName;
}

var myFunc = makeFunc();
myFunc(); //alert Mozilla

//------------------------

function makeAdder(x) {
  return function(y) {
    return x + y;
  };
}

var add5 = makeAdder(5);
var add10 = makeAdder(10);

console.log(add5(2));  // 7
console.log(add10(2)); // 12

//------------------------

// global scope
var e = 10;
function sum(a){
  return function(b){
    return function(c){
      // outer functions scope
      return function(d){
        // local scope
        return a + b + c + d + e;
      }
    }
  }
}

console.log(sum(1)(2)(3)(4)); // log 20

//------------------------

function makeFunc() {
  var name = 'Mozilla';
  function displayName() {
    alert(name);
  }
  name = 'Google';
  return displayName;  
}

var myFunc = makeFunc();
myFunc(); //alert Google

//------------------------

function getCounter() {
	var counter = 0;
	return function() {
		return ++counter;
	}
}

var counter = getCounter();
counter();
counter();
console.log(counter()); //3