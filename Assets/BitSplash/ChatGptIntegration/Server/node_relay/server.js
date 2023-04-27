const express = require('express') // require express
var https = require('https'); // require https
const rateLimit = require('express-rate-limit') // require rate limit
require('dotenv').config() // require environment variables config

// select a port
const PORT = process.env.PORT || 3000

//the api secret key comes from .env file and is the one you got from openai
const API_SECRET = process.env.API_SECRET
const API_ORGANIZATION = process.env.API_ORGANIZATION
const app = express()

// Rate limiting
const limiter = rateLimit({
  windowMs: 6000, // chatgpt uses a 1 minute window
  max: 3600, // 3600 requests per minute
})

//use the limiter and trust proxies
app.use(limiter)
app.set('trust proxy', 1)

app.post('/', function (req, res)
{
  var options = {
    hostname: 'api.openai.com',
    port: 443,
    path: '/v1/chat/completions',
    method: 'POST',
    headers: {
         'Authorization' : 'Bearer ' + API_SECRET,
         'Content-Type': 'application/json',
       }
  };
  if(API_ORGANIZATION)
  {
    options.headers['Organization'] = API_ORGANIZATION
  }
  res.contentType = 'application/json'
  var reqApi = https.request(options, (resApi) =>{
    resApi.pipe(res);
  });
  reqApi.on('error',(e) =>
  {
      console.log('error on api request ' + e.message)
      res.statusCode = e.statusCode
      res.send(e.message)
  });
  req.pipe(reqApi)
});

app.listen(PORT, () => console.log(`ChatGpt relay running on port ${PORT}`))