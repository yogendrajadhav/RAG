import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatCard, MatCardHeader, MatCardTitle, MatCardContent, MatCardSubtitle } from "@angular/material/card";
import { MatFormField, MatLabel, MatHint, MatError } from "@angular/material/form-field";
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinner } from "@angular/material/progress-spinner";
import { MatAccordion, MatExpansionPanel, MatExpansionPanelHeader, MatExpansionPanelTitle } from "@angular/material/expansion";
import { MatList, MatListItem } from "@angular/material/list";
import { DocumentService } from '../../services/documentservice';
import { QueryResponse } from '../../models/document';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-query-interface',
  imports: [CommonModule, FormsModule, MatCard, MatCardHeader, MatCardTitle, MatCardContent, MatFormField, MatLabel, MatHint, MatIcon, MatProgressSpinner, MatError, MatCardSubtitle, MatAccordion, MatExpansionPanel, MatExpansionPanelHeader, MatExpansionPanelTitle, MatList, MatListItem],
  templateUrl: './query-interface.html',
  styleUrl: './query-interface.scss',
})
export class QueryInterface {

  question = '';
  querying = false;
  queryResponse: QueryResponse | null = null;
  errorMessage: string | null = null;
  queryHistory: Array<{ question: string, response: QueryResponse }> = [];

  constructor(private documentService: DocumentService) { }

  onSubmitQuery(): void {
    if (!this.question.trim()) {
      this.errorMessage = 'Please enter a question';
      return;
    }
    this.querying = true;
    this.errorMessage = null;
    this.documentService.queryDocument(this.question).subscribe({
      next: (response) => {
        this.queryResponse = response;
        this.queryHistory.unshift({
          question: this.question,
          response: response
        });
        this.querying = false;
      },
      error: (error) => {
        this.errorMessage = 'Query failed: ' + error.message;
        this.querying = false;
      }
    });
  }

  clearQuery(): void {
    this.question = '';
    this.queryResponse = null;
    this.errorMessage = null;
  }
}

