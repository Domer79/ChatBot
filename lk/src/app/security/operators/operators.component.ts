import {Component, OnDestroy, OnInit} from '@angular/core';
import User from "../../contracts/user";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {OperatorEditDialogComponent} from "../operator-edit-dialog/operator-edit-dialog.component";
import {DialogResult} from "../../../abstracts/DialogResult";
import {OperatorService} from "../../services/operator.service";
import {Observable, Subscription} from "rxjs";
import {Security} from "../../security.decorator";
import {ActivatedRoute, Router} from "@angular/router";
import {map, tap} from "rxjs/operators";
import {PageEvent} from "@angular/material/paginator";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-operators',
  templateUrl: './operators.component.html',
  styleUrls: ['./operators.component.sass']
})
@Security('OperatorManager')
export class OperatorsComponent implements OnInit, OnDestroy {
  operators: Observable<User[]>;
  private queryParamsSubscription: Subscription;
  page: number = 1;
  size: number = 10;
  isActive?: string = undefined;

  totalCount: number;

  constructor(
      private userDialog: MatDialog,
      private operatorService: OperatorService,
      private route: ActivatedRoute,
      private router: Router,
      private snackBar: MatSnackBar,
  ) {
    // debugger;
  }

  ngOnInit(): void {
    this.queryParamsSubscription = this.route.queryParams.subscribe(queryParams => {
      if (!queryParams.page || !queryParams.size){
        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: { page: 1, size: 10 },
          replaceUrl: true
        });

        return;
      }

      this.page = queryParams['page'];
      this.size = queryParams['size'];
      this.isActive = queryParams['isactive'];

      this.updateList();
    });
  }

  private updateList(){
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
        debugger;
        this.updateList();
      }
    });
  }

  ngOnDestroy(): void {
    this.queryParamsSubscription.unsubscribe();
  }

  async activateOrBlock(operator: User): Promise<void> {
    if (operator.isActive){
      await this.operatorService.block(operator);
    }
    else{
      await this.operatorService.activate(operator);
    }

    this.snackBar.open(operator.isActive ? 'Оператор заблокирован' : 'Оператор активирован');
    this.updateList();
  }

  operatorEdit(operator: User) {
    this.openUserEditor(operator);
  }


  getPage($event: PageEvent) {
    console.log($event);
    this.page = $event.pageIndex + 1;
    this.size = $event.pageSize;

    const params = { page: this.page, size: this.size };
    if (this.isActive){
      params['isactive'] = this.isActive;
    }

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: params,
      replaceUrl: true
    })
  }
}
