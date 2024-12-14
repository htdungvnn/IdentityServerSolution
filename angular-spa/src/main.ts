import { importProvidersFrom } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AuthModule } from 'angular-auth-oidc-client';
import { AppComponent } from './app/app.component';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter([]),
    importProvidersFrom(AuthModule.forRoot({
      config: {
        authority: 'http://localhost:5000',
        redirectUrl: window.location.origin,
        clientId: 'spa-client',
        responseType: 'code',
        scope: 'openid profile api1',
        silentRenew: true,
      }
    }))
  ]
});
