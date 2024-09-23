import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Observable } from 'rxjs';
import { HackerNews } from '../models/hackerNews.model';
import { AsyncPipe, CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, AsyncPipe, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  http = inject(HttpClient)
  title = 'HackerNewsApp.Web';
  pageNumber: number = 1;
  pageSize: number = 10;
  searchTerm: string = ' ';
 
  news$ = this.getHackerNews(this.pageNumber, this.pageSize, this.searchTerm);

  private getHackerNews(pageNumber : number, pageSize: number, searchTerm: string): Observable<HackerNews[]>{
      return this.http.get<HackerNews[]>("https://localhost:7267/HackerNews?PageNumber="+pageNumber+"&pageSize="+pageSize
        +"&searchTerm=" + encodeURIComponent(searchTerm));
  }

  onSearch(event: Event): void {
    const filterValue = (event.target as HTMLInputElement)?.value;
    this.searchTerm = filterValue;
    this.news$ = this.getHackerNews(this.pageNumber, this.pageSize, filterValue);
    this.pageNumber = 1;
  }

  onPageChange(page: number): void {
    if(page > 0){
      this.pageNumber = page;
      this.news$ = this.getHackerNews(this.pageNumber, this.pageSize, this.searchTerm);
    }
  }
}
