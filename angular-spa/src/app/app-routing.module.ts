import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AutoLoginPartialRoutesGuard } from 'angular-auth-oidc-client';
import { AppComponent } from './app.component';

const routes: Routes = [
  { path: '', component: AppComponent }, // Public route
  {
    path: 'dashboard',
    component: AppComponent,
    canActivate: [AutoLoginPartialRoutesGuard],
  }, // Protected route
  {
    path: 'profile',
    component: AppComponent,
    canActivate: [AutoLoginPartialRoutesGuard],
  }, // Another protected route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
