var copyfiles = require('copyfiles');
 
copyfiles(['./src/node/*', './dist/'], '2', function(){});
copyfiles(['./src/node/node_modules/**', './dist/'], '2', function(){});