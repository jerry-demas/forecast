'use client';

import { useState } from 'react';
import DiceResults from './components/diceResults';
import DiceRoller from './components/diceRoller';
import type { DiceRollResult } from './interfaces/diceRoll';

export default function Dice() {

    const [rollResult, setRollResult] = useState<DiceRollResult>({ resultList: [] });

    const processResults = (result: DiceRollResult) => {
        setRollResult(result);
    };

    return (
        <div className="max-w-4xl mx-auto">
            <div className="mb-12 flex justify-between items-center">
                <div>
                    <h2 className="text-2xl font-bold text-gray-900">Roll some dice</h2>
                    <p className="text-gray-600">All your dice rolling needs.</p>
                </div>
            </div>           
            <div className="mb-12 flex justify-between items-center">
                <DiceRoller processResultCallback={processResults} />
            </div>
            <div className="mb-12 flex justify-between items-center">
                <DiceResults DiceRollResult={rollResult} />
            </div>
        </div>
    );
}
