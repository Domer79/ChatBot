import { Component, OnInit } from '@angular/core';
import {AuthService} from "../services/auth.service";
import {Router} from "@angular/router";
import {Observable} from "rxjs";
import {FormBuilder} from "@angular/forms";
import {MatSnackBar} from "@angular/material/snack-bar";
import {tap} from "rxjs/operators";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass']
})
export class LoginComponent implements OnInit {
  checkoutForm;
  subscribtions;

  constructor(
      private authService: AuthService,
      private router: Router,
      private formBuilder: FormBuilder,
      private _snackBar: MatSnackBar
  ) {
    this.checkoutForm = this.formBuilder.group({
      login: '',
      password: ''
    })
  }

  ngOnInit(): void {
  }

  logIn(data: any): void {
    debugger;
    this.authService.logIn(data.login, data.password).subscribe(async result => {
      debugger;
      if (!result) {
        this._snackBar.open(`${data.login} LogIn failed!`);
        return;
      }

      await this.router.navigate([this.authService.redirectUrl]);
    });
  }
}
