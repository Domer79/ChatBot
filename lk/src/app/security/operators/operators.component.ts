import {Component, OnDestroy, OnInit} from '@angular/core';
import User from "../../contracts/user";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {OperatorEditDialogComponent} from "../operator-edit-dialog/operator-edit-dialog.component";
import {DialogResult} from "../../../abstracts/DialogResult";
import {OperatorService} from "../../services/operator.service";
import {Observable, Subscription} from "rxjs";
import {Security} from "../../security.decorator";
import {ActivatedRoute} from "@angular/router";
import {map, tap} from "rxjs/operators";
import {DialogService} from "../../services/dialog.service";

@Component({
  selector: 'app-operators',
  templateUrl: './operators.component.html',
  styleUrls: ['./operators.component.sass']
})
@Security('OperatorManager')
export class OperatorsComponent implements OnInit, OnDestroy {
  operators: Observable<User[]>;
  private queryParamsSubscription: Subscription;
  private page: number = 1;
  private size: number = 10;
  private isActive?: boolean = null;

  totalCount: number;

  constructor(
      private userDialog: MatDialog,
      private operatorService: OperatorService,
      private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.queryParamsSubscription = this.route.queryParams.subscribe(queryParams => {
      this.page = queryParams['page'];
      this.size = queryParams['size'];
      this.isActive = queryParams['isactive'];
    });
  }

  private updatePage(){
    this.operators = this.operatorService.getPage(this.page, this.size, this.isActive).pipe(tap(p => {
      this.totalCount = p.totalCount;
    }), map(p => p.items));
  }

  openUserEditor(user: User | undefined = undefined) {
    var config = new MatDialogConfig();
    config.width = '900px';
    config.height = '700px';
    config.data = user ?? new User();
    this.userDialog.open(OperatorEditDialogComponent, config).afterClosed().subscribe(result => {
      if (result === DialogResult.Ok){
        this.updatePage();
      }
    });
  }

  ngOnDestroy(): void {
    this.queryParamsSubscription.unsubscribe();
  }

  activateOrBlock(operator: User) {

  }

  operatorEdit(operator: User) {

  }
}
