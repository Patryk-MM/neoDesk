const { env } = require('process');

// 1. Używamy 127.0.0.1 zamiast localhost, aby uniknąć problemów z Node.js (IPv6 vs IPv4)
// 2. Ustawiamy domyślny port na 40443 (zgodnie z Twoim launchSettings.json), a nie 7111
const target = env.ASPNETCORE_HTTPS_PORT ? `https://127.0.0.1:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://127.0.0.1:40443';

const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target: target, // TU BYŁ BŁĄD: Wcześniej ignorowałeś zmienną 'target'
    secure: false,  // Pozwala na użycie certyfikatów self-signed (development)
    changeOrigin: true // Często pomaga przy problemach z CORS/Host header
  }
]

module.exports = PROXY_CONFIG;

