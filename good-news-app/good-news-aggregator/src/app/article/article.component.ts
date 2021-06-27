import { Component, Input, OnInit } from '@angular/core';
import { Article } from 'src/models/article';
import { NewsService } from '../services/news.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.scss']
})
export class ArticleComponent implements OnInit {
  currentArticle?: Article = undefined;
  constructor(private newsService: NewsService, private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.newsService.getById(this.router.snapshot.params.id).subscribe(news => this.currentArticle = news); 
  }
}
