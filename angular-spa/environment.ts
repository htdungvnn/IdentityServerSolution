export const environment = {
  production: false,
  identityServer: {
    authority: 'https://localhost:5001',
    clientId: 'spa-client',
    redirectUrl: 'http://localhost:4200/auth-callback',
    postLogoutRedirectUri: 'http://localhost:4200/',
    responseType: 'code',
    scope: 'openid profile api1',
    silentRenew: true,
    useRefreshToken: true,
    secureRoutes: ['http://localhost:5001/api/'],
  },
};
