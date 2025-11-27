import { Component, inject, input, output, signal } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDeleteDialog } from '@shared/confirm-delete-dialog/confirm-delete-dialog';
import { Item } from '@shared/models/item';
import { ItemColumn } from '@shared/models/item-column';

@Component({
  selector: 'app-edit-item',
  standalone: false,
  templateUrl: './edit-item.html',
  styleUrl: './edit-item.sass',
})
export class EditItem {
  dialog = inject(MatDialog);
  item = input.required<Item>();
  columns = input.required<ItemColumn[]>();

  save = output<object>();
  cancel = output<void>();
  delete = output<object>();

  columnsToDisplay = signal<string[]>([]);
  readonly form = new FormBuilder().group({});

  ngOnInit() {
    this.columnsToDisplay.set(this.columns().filter(c => c.editable).map(c => c.name));
    for (let column of this.columns()) {
      if (column.editable && !this.form.contains(column.name)) {
        this.form.addControl(column.name, new FormBuilder().control('', Validators.required));
      }
    }
    if (this.item()) {
      this.form.patchValue(this.item()!);
    }
  }

  getLabel(column: string): string {
    const col = this.columns().find(c => c.name === column);
    return col ? col.label : column;
  }

  onSave() {
    if (this.form.valid) {
      let item = this.form.value as Item;      
      item.id = this.item()!.id;
      this.save.emit(item);
    }
  }

  onCancel() {
    if (this.item()) {
      this.form.patchValue(this.item()!);
    }
    this.cancel.emit();
  }

  onDelete() {
    if (!this.item()) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDeleteDialog);

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.delete.emit(this.item()!);
      }
    });
  }
}
