# Guidelines for AI Assistant to follow

## 1. Code Formatting
* Avoid writing "private" for private variables and methods.
* Private fields should be camelcase, prefixed with an underscore.
* Public fields should be PascalCase.
* Properties should be PascalCase.
* Curly braces should be on a new line.
* Static private fields should be camelcase, prefixed with 's_'.
* Static public fields should be camelcase.
* If a function only has "throw new NotImplementedException();" in it, replace that line with an empty line.