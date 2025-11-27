import { Component, input, output, signal } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ItemColumn } from '@shared/models/item-column';

@Component({
  selector: 'app-add-item',
  standalone: false,
  templateUrl: './add-item.html',
  styleUrl: './add-item.sass',
})
export class AddItem {
  columns = input.required<ItemColumn[]>();

  save = output<object>();
  cancel = output<void>();

  columnsToDisplay = signal<string[]>([]);
  readonly form = new FormBuilder().group({});

  ngOnChanges() {
    this.columnsToDisplay.set(this.columns().filter(c => c.editable).map(c => c.name));
    for (let column of this.columns()) {
      if (column.editable && !this.form.contains(column.name)) {
        this.form.addControl(column.name, new FormBuilder().control('', Validators.required));
      }
    }
  }

  getLabel(column: string): string {
    const col = this.columns().find(c => c.name === column);
    return col ? col.label : column;
  }

  onSave() {
    if (this.form.valid) {
      this.save.emit(this.form.value);
      this.form.reset();
    }
  }

  onCancel() {
    this.form.reset();
    this.cancel.emit();
  }
}
