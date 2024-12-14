import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginResponse, OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-root',
  template: `
    <h1>Angular 19 SPA</h1>
    <button *ngIf="!isAuthenticated" (click)="login()">Login</button>
    <button *ngIf="isAuthenticated" (click)="logout()">Logout</button>
  `,
  standalone: true,
})
export class AppComponent implements OnInit {
  private readonly oidcSecurityService = inject(OidcSecurityService);
  isAuthenticated = false;

  ngOnInit() {
    this.oidcSecurityService
      .checkAuth()
      .subscribe((loginResponse: LoginResponse) => {
        // const { isAuthenticated, userData, accessToken, idToken, configId } =
        //   loginResponse;
        this.isAuthenticated = loginResponse.isAuthenticated;
      });
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.oidcSecurityService
      .logoff()
      .subscribe((result) => console.log(result));
  }
}
