namespace Events
{
    // Событие, посылаемое когда внешний код считает текущий шаг туториала завершённым
    public struct TutorialStepCompleted
    {
        public Tutorial.TutorialStep Step;
    }
}
