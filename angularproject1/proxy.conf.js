const PROXY_CONFIG = [
  {
    context: ['/api1'],
    target: "https://localhost:5001",
    secure: false,
    logLevel: 'debug',
    pathRewrite: { '^/api1': '' }
  },
  {
    context: ['/api2'],
    target: "http://api.openweathermap.org",
    secure: false,
    logLevel: 'debug',
    pathRewrite: { '^/api2': '' }
  }
];
module.exports = PROXY_CONFIG;
