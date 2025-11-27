import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatExpansionModule } from '@angular/material/expansion';
import { ConfirmDeleteDialog } from './confirm-delete-dialog/confirm-delete-dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ItemList } from './item-list/item-list';
import { AddItem } from './add-item/add-item';
import { EditItem } from './edit-item/edit-item';
import { MatCardModule } from '@angular/material/card';
import { MatRadioModule } from '@angular/material/radio';
import { TopicService } from './services/topic-service';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { CardService } from './services/card-service';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';

@NgModule({
  declarations: [
    ConfirmDeleteDialog,
    ItemList,
    AddItem,
    EditItem
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatIconModule,
    MatButtonModule,
    MatDividerModule,
    MatTableModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatExpansionModule,
    MatDialogModule,
    MatTooltipModule,
    MatCardModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatButtonToggleModule,
    MatPaginatorModule,
    MatSortModule
  ], 
  exports: [
    ConfirmDeleteDialog,
    ItemList,
    AddItem,
    EditItem,
    MatIconModule,  
    MatButtonModule,
    MatDividerModule,
    MatTableModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatExpansionModule,
    MatDialogModule,
    MatTooltipModule,
    MatCardModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatButtonToggleModule,
    MatPaginatorModule,
    MatSortModule
  ],
  providers: [
    TopicService,
    CardService
  ]
})
export class SharedModule { }
