const http = require("http");
const fs = require("fs");
    
http.createServer(function(request, response){
      
    if(request.url == "/hello"){
        response.end("XMLHttpRequest на METANIT.COM");
    }
    else{
        fs.readFile("index.html", (error, data) => response.end(data));
    }
}).listen(5240, ()=>console.log("Сервер запущен по адресу http://localhost:5240"));