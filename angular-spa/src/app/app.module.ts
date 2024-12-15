import { NgModule } from '@angular/core';
import {
  BrowserModule,
  provideClientHydration,
  withEventReplay,
} from '@angular/platform-browser';

import { provideHttpClient, withFetch } from '@angular/common/http';
import { AuthModule } from 'angular-auth-oidc-client';
import { environment } from '../../environment';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
// ...
@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule.forRoot({
      config: {
        authority: environment.identityServer.authority,
        clientId: environment.identityServer.clientId,
        redirectUrl: environment.identityServer.redirectUrl,
        postLogoutRedirectUri: environment.identityServer.postLogoutRedirectUri,
        responseType: environment.identityServer.responseType,
        scope: environment.identityServer.scope,
        silentRenew: environment.identityServer.silentRenew,
        useRefreshToken: environment.identityServer.useRefreshToken,
        secureRoutes: environment.identityServer.secureRoutes,
      },
    }),
  ],
  providers: [
    provideClientHydration(withEventReplay()),
    provideHttpClient(withFetch()),
    provideAnimationsAsync(),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
