import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import { v4 as uuidv4 } from 'uuid';
import { NIL as guidEmpty } from 'uuid';
import User from "../../contracts/user";
import {AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators} from "@angular/forms";
import {userIdValidator} from "./user-id-validator";
import {OperatorService} from "../../services/operator.service";
import {tryCatch} from "rxjs/internal-compatibility";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Observable} from "rxjs";
import {Role} from "../../contracts/role";
import {MatCheckboxChange} from "@angular/material/checkbox";

@Component({
  selector: 'app-operator-edit-dialog',
  templateUrl: './operator-edit-dialog.component.html',
  styleUrls: ['./operator-edit-dialog.component.sass']
})
export class OperatorEditDialogComponent implements OnInit {
  action: string;
  userEditStepGroup: FormGroup;
  passwordGroup: FormGroup;
  roles: Observable<Role[]>;
  private isChanged: boolean;
  private passwordChanged: boolean;

  constructor(
      public dialogRef: MatDialogRef<OperatorEditDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: User,
      private _formBuilder: FormBuilder,
      private operatorService: OperatorService,
      private snackBar: MatSnackBar,
  ) { }

  ngOnInit(): void {
    this.action = this.data ? 'Реактирование' : 'Создание';
    this.userEditStepGroup = this._formBuilder.group({
      id: [this.data.id ?? guidEmpty, userIdValidator()],
      firstName: [this.data.firstName, Validators.required],
      lastName: [this.data.lastName, Validators.required],
      middleName: [this.data.middleName, Validators.nullValidator],
      login: [this.data.login, Validators.required],
      email: [this.data.email, Validators.required],
    });

    this.userEditStepGroup.valueChanges.subscribe(val => {
      this.isChanged = true;
    })

    this.passwordGroup = this._formBuilder.group({
      password: ['', Validators.nullValidator]
    });
    this.passwordGroup.controls.password.valueChanges.subscribe(value => {
      this.passwordChanged = true;
    })

    this.roles = this.operatorService.getRoles(this.userEditStepGroup.controls.id.value);
  }

  get userInfoIsSubmit(){
    const controls = this.userEditStepGroup.controls;
    for (const prop in controls){
      if (prop === "id") continue;
      if (controls[prop].status === "INVALID")
        return false;
    }

    return this.isChanged;
  }

  get isAllowTransitionToSetPassword(): boolean{
    if (!this.data.id)
      return false;

    if (this.isChanged)
      return false;

    return true;
  }

  get isAllowSetPassword(): boolean{
    return this.passwordChanged;
  }

  async userInfoSave() {
    try {
      console.log(this.userEditStepGroup.controls);
      const user = new User({
        id: this.userEditStepGroup.controls.id.value,
        firstName: this.userEditStepGroup.controls.firstName.value,
        lastName: this.userEditStepGroup.controls.lastName.value,
        middleName: this.userEditStepGroup.controls.middleName.value,
        login: this.userEditStepGroup.controls.login.value,
        email: this.userEditStepGroup.controls.email.value,
        isActive: true,
        isOperator: true,
      });

      const savedUser = await this.operatorService.upsert(user);
      this.userEditStepGroup.controls.id.setValue(savedUser.id);
      this.userEditStepGroup.updateValueAndValidity();

      this.isChanged = false;
    }
    catch (err: any){
      console.error(err);
      this.snackBar.open(err.message);
    }
  }

  async setPassword() {
    try {
      await this.operatorService.setPassword(this.userEditStepGroup.controls.id.value,
          this.passwordGroup.controls.password.value);
      this.passwordGroup.controls.password.setValue('');
      this.passwordChanged = false;
    }
    catch(err: any){
      console.error(err);
      this.snackBar.open(err.message);
    }
  }

  async roleChange(role: Role, $event: MatCheckboxChange) {
    const setted = await this.operatorService.setRole(role.id, this.userEditStepGroup.controls.id.value, $event.checked);
    if (setted){
      this.snackBar.open('Операция выполнена');
    }
  }
}
