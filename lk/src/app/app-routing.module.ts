import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {MainComponent} from "./main/main.component";
import {AuthGuard} from "./services/auth-guard.service";
import {AccessDeniedComponent} from "./access-denied/access-denied.component";
import {DialogsComponent} from "./dialogs/dialogs.component";

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'denied', component: AccessDeniedComponent},
  {
    path: '',
    canActivateChild: [AuthGuard],
    redirectTo: '/dialogs',
    pathMatch: 'full'
  },
  {path: 'dialogs', component: DialogsComponent, canActivate: [AuthGuard]},
  {path: 'dialogs/:id', component: DialogsComponent, canActivate: [AuthGuard]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
