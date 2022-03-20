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


  timeline: TimelineElement[] = [    
    { caption: '19 March', date: new Date(2022, 2, 19), title: 'Horizontal Timeline', content: this.content },
    { caption: '20 March', date: new Date(2022, 2, 20), title: 'Horizontal Timeline', selected: true, content: this.content },
  ];
}
