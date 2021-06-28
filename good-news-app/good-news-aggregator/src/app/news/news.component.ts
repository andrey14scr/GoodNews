import { Component, OnInit } from '@angular/core';
import { Article } from 'src/models/article';
import { ArticleModel } from 'src/models/article-model';
import { NewsService } from 'src/app/services/news.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss']
})
export class NewsComponent implements OnInit {
  newsCollection: ArticleModel[] = [];
  selectedArticle?: ArticleModel;

  constructor(private newsService: NewsService, private router: Router) { }

  ngOnInit(): void {
    this.getNews();
  }

  getNews() : void{
    this.newsService.getNews().subscribe(news => this.newsCollection = news); 
  }

  Select(article: ArticleModel) : void{

    this.newsCollection.forEach(element => {  
      if(element){
        const app = document.getElementById('a-' + element.id.toString());
        app?.remove();
      }
    });

    if(this.selectedArticle != article){
      this.selectedArticle = article;
      
      const app = document.getElementById(article.id.toString());
      const p = document.createElement("div");
      p.innerHTML = '<a id='+ 'a-' + article.id.toString() +' href="/article/'+ article.id.toString() +'">Читать</a>';
      app?.appendChild(p);
    }
    else{
      this.selectedArticle = undefined;
    }
  }
}
