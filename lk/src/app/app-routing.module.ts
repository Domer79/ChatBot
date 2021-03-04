import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {MainComponent} from "./main/main.component";
import {AuthGuard} from "./services/auth-guard.service";
import {AccessDeniedComponent} from "./access-denied/access-denied.component";
import {DialogsComponent} from "./dialogs/dialogs.component";
import {QuestionsPageComponent} from "./questions/questions-page/questions-page.component";
import {OperatorsComponent} from "./security/operators/operators.component";
import {LinkType} from "./contracts/message-dialog";
import {TestComponent} from "./test/test.component";
import {AppComponent} from "./app/app.component";

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'denied', component: AccessDeniedComponent},
  {path: '', component: MainComponent, children: [
      {
        path: '',
        canActivateChild: [AuthGuard],
        redirectTo: `/dialogs/${LinkType[LinkType.all]}`,
        pathMatch: 'full'
      },
      {
        path: 'dialogs',
        canActivateChild: [AuthGuard],
        redirectTo: `dialogs/${LinkType[LinkType.all]}`,
        pathMatch: 'full'
      },
      {path: 'dialogs/:id', component: DialogsComponent, canActivate: [AuthGuard], pathMatch: 'full'},
      {path: 'questions', component: QuestionsPageComponent, canActivate: [AuthGuard]},
      {path: 'questions/:parentId', component: QuestionsPageComponent, canActivate: [AuthGuard], pathMatch: 'full'},
      {path: 'operators', component: OperatorsComponent, canActivate: [AuthGuard]},
      {path: 'test', component: TestComponent, canActivate: [AuthGuard]},
    ]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
