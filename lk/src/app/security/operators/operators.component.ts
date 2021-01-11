import { Component, OnInit } from '@angular/core';
import User from "../../contracts/user";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {OperatorEditDialogComponent} from "../operator-edit-dialog/operator-edit-dialog.component";
import {DialogResult} from "../../../abstracts/DialogResult";
import {OperatorService} from "../../services/operator.service";
import {Observable} from "rxjs";

@Component({
  selector: 'app-operators',
  templateUrl: './operators.component.html',
  styleUrls: ['./operators.component.sass']
})
export class OperatorsComponent implements OnInit {
  private operators: Observable<User[]>;

  constructor(
      private userDialog: MatDialog,
      private operatorService: OperatorService
  ) { }

  ngOnInit(): void {
    this.operators = this.operatorService.getAll();
  }

  openUserEditor(user: User | undefined = undefined) {
    var config = new MatDialogConfig();
    config.width = '900px';
    config.height = '700px';
    config.data = user;
    this.userDialog.open(OperatorEditDialogComponent, config).afterClosed().subscribe(result => {
      if (result === DialogResult.Ok){
        this.operators = this.operatorService.getAll();
      }
    });
  }
}
