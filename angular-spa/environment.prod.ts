export const environment = {
  production: true,
  identityServer: {
    authority: 'https://your-production-identityserver-url',
    clientId: 'spa-client',
    redirectUrl: 'https://your-production-app-url/auth-callback',
    postLogoutRedirectUri: 'https://your-production-app-url/',
    responseType: 'code',
    scope: 'openid profile api1',
    silentRenew: true,
    useRefreshToken: true,
    secureRoutes: ['https://your-production-api-url/api/'],
  },
};
