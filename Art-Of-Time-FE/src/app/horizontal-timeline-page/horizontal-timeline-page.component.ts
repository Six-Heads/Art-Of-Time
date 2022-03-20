import { Component } from '@angular/core';
import {TimelineElement} from "./horizontal-timeline/timeline-element";

@Component({
  selector: 'app-horizontal-timeline-page',
  templateUrl: './horizontal-timeline-page.component.html',
  styleUrls: ['./horizontal-timeline-page.component.css']
})
export class HorizontalTimelinePageComponent{
  name = 'Angular 6';
  content = `Lorem ipsum dolor sit amet, consectetur adipisicing elit. Illum praesentium officia, fugit recusandae
    ipsa, quia velit nulla adipisci? Consequuntur aspernatur at, eaque hic repellendus sit dicta consequatur quae,
    ut harum ipsam molestias maxime non nisi reiciendis eligendi! Doloremque quia pariatur harum ea amet quibusdam
    quisquam, quae, temporibus dolores porro doloribus.`;


  timeline: TimelineElement[] =
    [
      {id: 1, caption: '16 Jan AM', date: new Date(2014, 0, 16), title: '16 Jan AM', content: this.content },
      {id: 2,  caption: '28 Feb PM', date: new Date(2014, 1, 28), selected: true, title: '28 Feb PM', content: ''},
      {id: 3,  caption: '20 Mar AM', date: new Date(2014, 2, 20), title: '20 Mar AM', content: this.content },
      {id: 4,  caption: '20 May AM', date: new Date(2014, 4, 20), title: 'Status#3', content: this.content },
      {id: 5,  caption: '09 Jul AM', date: new Date(2014, 6, 9), title: 'Status#4', content: this.content },
      {id: 6,  caption: '30 Aug', date: new Date(2014, 8, 30), title: 'Status#5', content: this.content },
      {id: 7,  caption: '15 Sep', date: new Date(2014, 9, 15), title: 'Status#6', content: this.content },
      {id: 8,  caption: '01 Nov', date: new Date(2014, 11, 1), title: 'Status#7', content: this.content },
      {id: 9,  caption: '10 Dec', date: new Date(2014, 12, 10), title: 'Status#8', content: this.content },
      {id: 10,  caption: '29 Jan', date: new Date(2015, 1, 19), title: 'Status#9', content: this.content },
      {id: 11,  caption: '3 Mar', date: new Date(2015, 3, 3), title: 'Status#9', content: this.content },
    ];

}
