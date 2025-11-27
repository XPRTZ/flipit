import { Component, effect, input, output, signal, TemplateRef, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ItemColumn } from '@shared/models/item-column';

@Component({
  selector: 'app-item-list',
  standalone: false,
  templateUrl: './item-list.html',
  styleUrl: './item-list.sass',
})
export class ItemList {
  name = input.required<string>();
  items = input.required<object[]>();
  columns = input.required<ItemColumn[]>();
  editable = input<boolean>(true);
  valueTemplate = input<TemplateRef<any>>();
  actionsTemplate = input<TemplateRef<any>>();

  add = output<object>();
  edit = output<object>();
  cancel = output<void>();
  delete = output<object>();

  columnsToDisplay = signal<string[]>([]);
  columnsToDisplayWithExpand = signal<string[]>([]);
  expandedItem: object | null = null;

  dataSource = new MatTableDataSource<object>([]);

  @ViewChild(MatSort) sort: MatSort | undefined;
  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;

  constructor() {
    effect(() => {
      this.dataSource.data = this.items();
    });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  ngOnChanges() {
    this.columnsToDisplay.set(this.columns().map(c => c.name));
    this.columnsToDisplayWithExpand.set([...this.columnsToDisplay(), 'expand']);
  }

  getLabel(column: string): string {
    const col = this.columns().find(c => c.name === column);
    return col ? col.label : column;
  }

  isExpanded(item: object) {
    return this.expandedItem === item;
  }

  toggle(item: object) {
    if (this.editable()) {
      this.expandedItem = this.isExpanded(item) ? null : item;
    }
  }

  addItem(item: object) {
    this.add.emit(item);
  }

  editItem(item: object) {
    this.edit.emit(item);
  }

  cancelItem() {
    this.expandedItem = null;
    this.cancel.emit();
  }

  deleteItem(item: object) {
    this.delete.emit(item);
  }
}
