const { env } = require('process');

const target = env.MEISHI_API_URL ?? 'http://localhost:80';

const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
