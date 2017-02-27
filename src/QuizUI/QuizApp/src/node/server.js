var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');

var app = express();

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(__dirname));

// get environment vars
var port = process.env.PORT || 8080;
var apibaseurl = process.env.APIBASEURL || "(no endpoint in env vars)";

// serve api base url
app.get('/apibaseurl', function (req, res) {
    console.log('QuizApp received request for API base url');
    res.json({apibaseurl: apibaseurl});
});

// use index.html as our root
app.get('/*', function (req, res) {
    console.log('QuizApp received request for root - serving up index.html from: ' + path.join(__dirname,'index.html'));
    res.sendFile(path.join(__dirname,'index.html'));
});

// catch 404 and forward to error handler
app.use(function(req, res, next) {
    var err = new Error('Not Found');
    err.status = 404;
    next(err);
});

app.listen(port, function () {
    console.log('QuizApp listening on port '+ port + ' !');
});


module.exports = app;
