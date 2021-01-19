import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {NIL as guidEmpty} from 'uuid';
import User from "../../contracts/user";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {userIdValidator} from "./user-id-validator";
import {OperatorService} from "../../services/operator.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Observable} from "rxjs";
import {Role} from "../../contracts/role";
import {MatCheckboxChange} from "@angular/material/checkbox";
import {DialogResult} from "../../../abstracts/DialogResult";

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
  dialogResult: DialogResult;

  constructor(
      public dialogRef: MatDialogRef<OperatorEditDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: User,
      private _formBuilder: FormBuilder,
      private operatorService: OperatorService,
      private snackBar: MatSnackBar,
  ) {
    this.dialogRef.disableClose = true;
  }

  ngOnInit(): void {
    this.action = this.getAction();
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

  private getAction(): string{
    if (!this.data){
      return 'Создание';
    }

    if (!this.data.id || this.data.id === guidEmpty){
      return 'Создание';
    }

    if (this.data.id.match(/[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/i))
      return "Редактирование";

    return 'Создание';
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
        isActive: this.data.isActive ?? true,
        isOperator: this.data.isOperator ?? true,
        dateCreated: this.data.dateCreated,
        dateBlocked: this.data.dateBlocked,
        number: this.data.number,
      });

      const savedUser = await this.operatorService.upsert(user);
      this.userEditStepGroup.controls.id.setValue(savedUser.id);
      this.userEditStepGroup.updateValueAndValidity();

      this.isChanged = false;
      this.snackBar.open("Сведения сохранены");
      this.dialogResult = DialogResult.Ok;
    }
    catch (err: any){
      console.error(err);
      this.snackBar.open(err.message);
      this.dialogResult = DialogResult.Error;
    }
  }

  async setPassword() {
    try {
      await this.operatorService.setPassword(this.userEditStepGroup.controls.id.value,
          this.passwordGroup.controls.password.value);
      this.passwordGroup.controls.password.setValue('');
      this.passwordChanged = false;
      this.snackBar.open('Пароль сохранен');
    }
    catch(err: any){
      console.error(err);
      this.snackBar.open(err.message);
      this.dialogResult = DialogResult.Error;
    }
  }

  async roleChange(role: Role, $event: MatCheckboxChange) {
    const setted = await this.operatorService.setRole(role.id, this.userEditStepGroup.controls.id.value, $event.checked);
    if (setted){
      this.snackBar.open($event.checked ? 'Роль установлена' : 'Роль снята');
    }
    else{
      console.log('Не получилось установить роль');
      this.dialogResult = DialogResult.Error;
    }
  }

  closeDialog() {
    this.dialogRef.close(this.dialogResult);
  }
}
