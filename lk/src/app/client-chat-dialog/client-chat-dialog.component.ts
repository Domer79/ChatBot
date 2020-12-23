import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-client-chat-dialog',
  templateUrl: './client-chat-dialog.component.html',
  styleUrls: ['./client-chat-dialog.component.sass']
})
export class ClientChatDialogComponent implements OnInit {

  constructor(
      public dialogRef: MatDialogRef<ClientChatDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
