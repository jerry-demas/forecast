'use client';

import type { DiceRollResult } from '../interfaces/diceRoll';

export interface DiceResultsProps {
	readonly DiceRollResult: DiceRollResult;
}
export default function DiceResults({ DiceRollResult }: DiceResultsProps) {
	return (
		<div className="">
			<div className="items-left">
				<div>
					<p className="text-gray-600">Results:</p>
				</div>
			</div>
			<div className="items-left">
				<ul>
					{DiceRollResult.resultList?.map((roll, index) => (
						<li key={index}>
							{roll}
						</li>
					))}
				</ul>
			</div>
		</div>
	);
}
