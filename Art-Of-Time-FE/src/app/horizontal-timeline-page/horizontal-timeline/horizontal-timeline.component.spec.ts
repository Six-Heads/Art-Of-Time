import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HorizontalTimelineComponent } from './horizontal-timeline.component';

describe('HorizontalTimelineComponent', () => {
  let component: HorizontalTimelineComponent;
  let fixture: ComponentFixture<HorizontalTimelineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HorizontalTimelineComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HorizontalTimelineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
