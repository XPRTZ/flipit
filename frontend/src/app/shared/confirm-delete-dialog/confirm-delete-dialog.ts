import { Component } from '@angular/core';

@Component({
  selector: 'app-confirm-delete-dialog',
  standalone: false,
  templateUrl: './confirm-delete-dialog.html',
  styleUrl: './confirm-delete-dialog.sass',
})
export class ConfirmDeleteDialog {
  onOk() {
    return true;
  }
  onNo() {
    return false;
  }
}
